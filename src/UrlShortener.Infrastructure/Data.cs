using Microsoft.EntityFrameworkCore;
using UrlShortener.Domain.Entities;

namespace UrlShortener.Infrastructure.Data;

public sealed class UrlShortenerDbContext(DbContextOptions<UrlShortenerDbContext> options) : DbContext(options)
{
    public DbSet<ShortUrl> ShortUrls => Set<ShortUrl>();
    public DbSet<ClickEvent> ClickEvents => Set<ClickEvent>();

    protected override void OnModelCreating(ModelBuilder b)
    {
        b.Entity<ShortUrl>(e =>
        {
            e.ToTable("ShortUrls");
            e.HasKey(x => x.Id);
            e.Property(x => x.ShortCode).HasMaxLength(20).IsRequired();
            e.Property(x => x.OriginalUrl).HasMaxLength(2048).IsRequired();
            e.HasIndex(x => x.ShortCode).IsUnique();
            e.Property(x => x.ClickCount).IsRequired();
            e.HasMany(x => x.ClickEvents)
                .WithOne(x => x.ShortUrl)
                .HasForeignKey(x => x.ShortUrlId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        b.Entity<ClickEvent>(e =>
        {
            e.ToTable("ClickEvents");
            e.HasKey(x => x.Id);
            e.Property(x => x.IpAddressHash).HasMaxLength(64);
            e.Property(x => x.UserAgent).HasMaxLength(1000);
            e.Property(x => x.Referrer).HasMaxLength(2048);
            e.HasIndex(x => new { x.ShortUrlId, x.ClickedAtUtc });
        });
    }
}