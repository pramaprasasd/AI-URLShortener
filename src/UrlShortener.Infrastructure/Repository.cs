using Microsoft.EntityFrameworkCore;
using UrlShortener.Application.Contracts;
using UrlShortener.Domain.Entities;
using UrlShortener.Infrastructure.Data;

namespace UrlShortener.Infrastructure.Repositories;

public sealed class ShortUrlRepository(UrlShortenerDbContext db) : IShortUrlRepository
{
    public Task<bool> ExistsAsync(string code, CancellationToken ct) =>
        db.ShortUrls.AsNoTracking().AnyAsync(x => x.ShortCode == code, ct);

    public async Task<ShortUrl> AddAsync(ShortUrl entity, CancellationToken ct)
    {
        db.ShortUrls.Add(entity);
        await db.SaveChangesAsync(ct);
        return entity;
    }

    public Task<ShortUrl?> GetByCodeAsync(string code, CancellationToken ct) =>
        db.ShortUrls.AsNoTracking().SingleOrDefaultAsync(x => x.ShortCode == code, ct);

    public async Task AddClickAsync(ClickEvent click, CancellationToken ct)
    {
        // Keep the counter update atomic at the database level.
        await using var transaction = await db.Database.BeginTransactionAsync(ct);

        db.ClickEvents.Add(click);
        await db.Database.ExecuteSqlInterpolatedAsync(
            $"UPDATE ShortUrls SET ClickCount = ClickCount + 1 WHERE Id = {click.ShortUrlId}",
            ct);

        await db.SaveChangesAsync(ct);
        await transaction.CommitAsync(ct);
    }

    public async Task<AnalyticsResponse?> GetAnalyticsAsync(long id, CancellationToken ct)
    {
        var url = await db.ShortUrls.AsNoTracking()
            .SingleOrDefaultAsync(x => x.Id == id, ct);

        if (url is null)
            return null;

        var q = db.ClickEvents.AsNoTracking()
            .Where(x => x.ShortUrlId == id);

        var first = await q.MinAsync(x => (DateTime?)x.ClickedAtUtc, ct);
        var last = await q.MaxAsync(x => (DateTime?)x.ClickedAtUtc, ct);

        var refs = await q
            .Where(x => x.Referrer != null && x.Referrer != "")
            .GroupBy(x => x.Referrer!)
            .OrderByDescending(g => g.LongCount())
            .Take(10)
            .Select(g => new { Referrer = g.Key, Count = g.LongCount() })
            .ToListAsync(ct);

        return new AnalyticsResponse(
            url.Id,
            url.ShortCode,
            url.ClickCount,
            first,
            last,
            refs.ToDictionary(x => x.Referrer, x => x.Count));
    }
}