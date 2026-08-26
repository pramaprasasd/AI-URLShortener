using UrlShortener.Domain.Entities;

namespace UrlShortener.UnitTests;

public class ShortUrlEntityTests
{
    [Fact]
    public void IsExpired_ReturnsFalse_WhenNoExpiration()
    {
        var url = new ShortUrl { ExpiresAtUtc = null };
        Assert.False(url.IsExpired(DateTime.UtcNow));
    }

    [Fact]
    public void IsExpired_ReturnsTrue_WhenExpirationIsInPast()
    {
        var url = new ShortUrl { ExpiresAtUtc = DateTime.UtcNow.AddMinutes(-1) };
        Assert.True(url.IsExpired(DateTime.UtcNow));
    }

    [Fact]
    public void IsExpired_ReturnsFalse_WhenExpirationIsInFuture()
    {
        var url = new ShortUrl { ExpiresAtUtc = DateTime.UtcNow.AddMinutes(1) };
        Assert.False(url.IsExpired(DateTime.UtcNow));
    }
}
