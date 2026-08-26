using System.ComponentModel.DataAnnotations;
using UrlShortener.Domain.Entities;

namespace UrlShortener.Application.Contracts;

public sealed record CreateShortUrlRequest(
    [property: Required] string OriginalUrl,
    [property: StringLength(20, MinimumLength = 3)] string? CustomAlias,
    DateTime? ExpiresAtUtc);

public sealed record ShortUrlResponse(
    long Id,
    string ShortCode,
    string ShortUrl,
    string OriginalUrl,
    DateTime CreatedAtUtc,
    DateTime? ExpiresAtUtc,
    long ClickCount);

public sealed record AnalyticsResponse(
    long UrlId,
    string ShortCode,
    long TotalClicks,
    DateTime? FirstClickedAtUtc,
    DateTime? LastClickedAtUtc,
    IReadOnlyDictionary<string, long> TopReferrers);

public interface IShortUrlService
{
    Task<ShortUrlResponse> CreateAsync(CreateShortUrlRequest request, string baseUrl, CancellationToken ct);
    Task<ResolvedUrl?> ResolveAsync(string code, CancellationToken ct);
    Task RecordClickAsync(long id, string? ipAddress, string? userAgent, string? referrer, CancellationToken ct);
    Task<AnalyticsResponse?> GetAnalyticsAsync(long id, CancellationToken ct);
}

public sealed record ResolvedUrl(long Id, string OriginalUrl);

public interface IShortCodeGenerator
{
    string Generate(int length = 7);
}

public interface IShortUrlRepository
{
    Task<bool> ExistsAsync(string code, CancellationToken ct);
    Task<ShortUrl> AddAsync(ShortUrl entity, CancellationToken ct);
    Task<ShortUrl?> GetByCodeAsync(string code, CancellationToken ct);
    Task AddClickAsync(ClickEvent click, CancellationToken ct);
    Task<AnalyticsResponse?> GetAnalyticsAsync(long id, CancellationToken ct);
}