using Microsoft.Extensions.Caching.Memory;
using Moq;
using System.ComponentModel.DataAnnotations;
using UrlShortener.Application.Contracts;
using UrlShortener.Application.Services;
using UrlShortener.Domain.Entities;

namespace UrlShortener.UnitTests;

public sealed class ShortUrlServiceTests
{
    private readonly Mock<IShortUrlRepository> _repo = new();
    private readonly Mock<IShortCodeGenerator> _generator = new();
    private readonly MemoryCache _cache = new(new MemoryCacheOptions());
    private readonly TimeProvider _clock = TimeProvider.System;

    [Fact]
    public async Task CreateAsync_CreatesRandomShortUrl()
    {
        _generator.Setup(x => x.Generate(It.IsAny<int>())).Returns("AbC1234");
        _repo.Setup(x => x.ExistsAsync("AbC1234", It.IsAny<CancellationToken>())).ReturnsAsync(false);
        _repo.Setup(x => x.AddAsync(It.IsAny<ShortUrl>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((ShortUrl x, CancellationToken _) =>
            {
                x.Id = 10;
                return x;
            });

        var sut = new ShortUrlService(_repo.Object, _generator.Object, _cache, _clock);

        var result = await sut.CreateAsync(
            new CreateShortUrlRequest("https://example.com/a", null, null),
            "https://short.example",
            CancellationToken.None);

        Assert.Equal(10, result.Id);
        Assert.Equal("AbC1234", result.ShortCode);
        Assert.Equal("https://short.example/r/AbC1234", result.ShortUrl);
    }

    [Fact]
    public async Task CreateAsync_RejectsNonHttpUrl()
    {
        var sut = new ShortUrlService(_repo.Object, _generator.Object, _cache, _clock);

        await Assert.ThrowsAsync<ValidationException>(() =>
            sut.CreateAsync(
                new CreateShortUrlRequest("javascript:alert(1)", null, null),
                "https://short.example",
                CancellationToken.None));
    }

    [Fact]
    public async Task CreateAsync_RejectsDuplicateCustomAlias()
    {
        _repo.Setup(x => x.ExistsAsync("custom", It.IsAny<CancellationToken>())).ReturnsAsync(true);

        var sut = new ShortUrlService(_repo.Object, _generator.Object, _cache, _clock);

        await Assert.ThrowsAsync<InvalidOperationException>(() =>
            sut.CreateAsync(
                new CreateShortUrlRequest("https://example.com", "custom", null),
                "https://short.example",
                CancellationToken.None));
    }

    [Fact]
    public async Task ResolveAsync_UsesCacheAfterFirstLookup()
    {
        var entity = new ShortUrl
        {
            Id = 1,
            ShortCode = "abc1234",
            OriginalUrl = "https://example.com",
            CreatedAtUtc = DateTime.UtcNow,
            IsActive = true
        };

        _repo.Setup(x => x.GetByCodeAsync("abc1234", It.IsAny<CancellationToken>()))
            .ReturnsAsync(entity);

        var sut = new ShortUrlService(_repo.Object, _generator.Object, _cache, _clock);

        var first = await sut.ResolveAsync("abc1234", CancellationToken.None);
        var second = await sut.ResolveAsync("abc1234", CancellationToken.None);

        Assert.Equal("https://example.com", first?.OriginalUrl);
        Assert.Equal("https://example.com", second?.OriginalUrl);

        _repo.Verify(
            x => x.GetByCodeAsync("abc1234", It.IsAny<CancellationToken>()),
            Times.Once);
    }

    [Fact]
    public async Task ResolveAsync_ReturnsNullForExpiredUrl()
    {
        var entity = new ShortUrl
        {
            Id = 1,
            ShortCode = "expired",
            OriginalUrl = "https://example.com",
            CreatedAtUtc = DateTime.UtcNow.AddHours(-2),
            ExpiresAtUtc = DateTime.UtcNow.AddMinutes(-1),
            IsActive = true
        };

        _repo.Setup(x => x.GetByCodeAsync("expired", It.IsAny<CancellationToken>()))
            .ReturnsAsync(entity);

        var sut = new ShortUrlService(_repo.Object, _generator.Object, _cache, _clock);

        var result = await sut.ResolveAsync("expired", CancellationToken.None);

        Assert.Null(result);
    }
}