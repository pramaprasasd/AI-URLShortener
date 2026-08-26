using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using UrlShortener.Application.Contracts;
using UrlShortener.Domain.Entities;

namespace UrlShortener.Application.Services;

public sealed class ShortUrlService(
    IShortUrlRepository repository,
    IShortCodeGenerator codeGenerator,
    IMemoryCache cache,
    TimeProvider timeProvider) : IShortUrlService
{
    private static string CacheKey(string code) => $"short-url:{code}";
    private static readonly TimeSpan CacheDuration = TimeSpan.FromMinutes(10);

    public async Task<ShortUrlResponse> CreateAsync(
        CreateShortUrlRequest request,
        string baseUrl,
        CancellationToken ct)
    {
        ValidateRequest(request);

        var code = string.IsNullOrWhiteSpace(request.CustomAlias)
            ? await GenerateUniqueCodeAsync(ct)
            : request.CustomAlias.Trim();

        if (!string.IsNullOrWhiteSpace(request.CustomAlias) &&
            await repository.ExistsAsync(code, ct))
        {
            throw new InvalidOperationException("The short code is already in use.");
        }

        var entity = new ShortUrl
        {
            ShortCode = code,
            OriginalUrl = NormalizeUrl(request.OriginalUrl),
            CreatedAtUtc = timeProvider.GetUtcNow().UtcDateTime,
            ExpiresAtUtc = request.ExpiresAtUtc,
            IsActive = true,
            ClickCount = 0
        };

        try
        {
            entity = await repository.AddAsync(entity, ct);
        }
        catch (DbUpdateException ex)
        {
            // The database unique constraint remains the final concurrency guard.
            throw new ShortCodeConflictException("The short code is already in use.", ex);
        }

        return ToResponse(entity, baseUrl);
    }

    public async Task<ResolvedUrl?> ResolveAsync(string code, CancellationToken ct)
    {
        if (string.IsNullOrWhiteSpace(code) || code.Length > 20)
            return null;

        if (cache.TryGetValue(CacheKey(code), out ShortUrl? cached) && cached is not null)
        {
            return IsRedirectable(cached)
                ? new ResolvedUrl(cached.Id, cached.OriginalUrl)
                : null;
        }

        var entity = await repository.GetByCodeAsync(code, ct);

        if (entity is null || !IsRedirectable(entity))
            return null;

        cache.Set(CacheKey(code), entity, CacheDuration);
        return new ResolvedUrl(entity.Id, entity.OriginalUrl);
    }

    public Task RecordClickAsync(
        long id,
        string? ipAddress,
        string? userAgent,
        string? referrer,
        CancellationToken ct)
    {
        return repository.AddClickAsync(new ClickEvent
        {
            ShortUrlId = id,
            ClickedAtUtc = timeProvider.GetUtcNow().UtcDateTime,
            IpAddressHash = HashIpAddress(ipAddress),
            UserAgent = Truncate(userAgent, 1000),
            Referrer = Truncate(referrer, 2048)
        }, ct);
    }

    public Task<AnalyticsResponse?> GetAnalyticsAsync(long id, CancellationToken ct) =>
        repository.GetAnalyticsAsync(id, ct);

    private async Task<string> GenerateUniqueCodeAsync(CancellationToken ct)
    {
        for (var attempt = 0; attempt < 5; attempt++)
        {
            var code = codeGenerator.Generate();
            if (!await repository.ExistsAsync(code, ct))
                return code;
        }

        throw new InvalidOperationException("Unable to allocate a unique short code.");
    }

    private static bool IsRedirectable(ShortUrl url) =>
        url.IsActive && !url.IsExpired(DateTime.UtcNow);

    private static void ValidateRequest(CreateShortUrlRequest request)
    {
        if (string.IsNullOrWhiteSpace(request.OriginalUrl))
            throw new ValidationException("OriginalUrl is required.");

        if (request.OriginalUrl.Length > 2048)
            throw new ValidationException("OriginalUrl must not exceed 2048 characters.");

        if (!Uri.TryCreate(request.OriginalUrl, UriKind.Absolute, out var uri) ||
            (uri.Scheme != Uri.UriSchemeHttp && uri.Scheme != Uri.UriSchemeHttps))
        {
            throw new ValidationException("Only absolute HTTP/HTTPS URLs are supported.");
        }

        if (!string.IsNullOrWhiteSpace(request.CustomAlias) &&
            request.CustomAlias.Any(c => !char.IsLetterOrDigit(c) && c != '-' && c != '_'))
        {
            throw new ValidationException("CustomAlias may contain only letters, numbers, '-' and '_'.");
        }

        if (request.ExpiresAtUtc.HasValue &&
            request.ExpiresAtUtc.Value <= DateTime.UtcNow)
        {
            throw new ValidationException("ExpiresAtUtc must be in the future.");
        }
    }

    private static string NormalizeUrl(string value) => new Uri(value.Trim()).ToString();

    private static string? Truncate(string? value, int maxLength) =>
        string.IsNullOrEmpty(value) ? value : value[..Math.Min(value.Length, maxLength)];

    private static string? HashIpAddress(string? ipAddress)
    {
        if (string.IsNullOrWhiteSpace(ipAddress))
            return null;

        var bytes = System.Security.Cryptography.SHA256.HashData(
            System.Text.Encoding.UTF8.GetBytes(ipAddress));
        return Convert.ToHexString(bytes);
    }

    private static ShortUrlResponse ToResponse(ShortUrl x, string baseUrl) =>
        new(
            x.Id,
            x.ShortCode,
            $"{baseUrl.TrimEnd('/')}/r/{x.ShortCode}",
            x.OriginalUrl,
            x.CreatedAtUtc,
            x.ExpiresAtUtc,
            x.ClickCount);
}

public sealed class ShortCodeConflictException(string message, Exception innerException)
    : Exception(message, innerException);