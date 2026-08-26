namespace UrlShortener.Domain.Entities;

public sealed class ShortUrl
{
    public long Id { get; set; }
    public string ShortCode { get; set; } = null!;
    public string OriginalUrl { get; set; } = null!;
    public DateTime CreatedAtUtc { get; set; }
    public DateTime? ExpiresAtUtc { get; set; }
    public bool IsActive { get; set; } = true;
    public long ClickCount { get; set; }
    public ICollection<ClickEvent> ClickEvents { get; set; } = new List<ClickEvent>();

    public bool IsExpired(DateTime utcNow) =>
        ExpiresAtUtc.HasValue && ExpiresAtUtc.Value <= utcNow;
}

public sealed class ClickEvent
{
    public long Id { get; set; }
    public long ShortUrlId { get; set; }
    public DateTime ClickedAtUtc { get; set; }
    public string? IpAddressHash { get; set; }
    public string? UserAgent { get; set; }
    public string? Referrer { get; set; }
    public ShortUrl ShortUrl { get; set; } = null!;
}