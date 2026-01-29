using HsoPkipt.Identity;
using HsoPkipt.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace HsoPkipt.Data;

public class AppDbContext : IdentityDbContext<AppUser, AppRole, Guid>
{
    public AppDbContext(DbContextOptions<AppDbContext> options)
    : base(options)
    {
    }

    public DbSet<NewsItem> News => Set<NewsItem>();
    public DbSet<Tag> Tags => Set<Tag>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Конфигурация сущностей
        modelBuilder.Entity<Tag>().HasMany(t => t.News).WithMany(n => n.Tags);

        // Seed Tags
        var tag1 = new Tag("Technology");
        var tag2 = new Tag("Education");
        var tag3 = new Tag("Gaming");

        modelBuilder.Entity<Tag>().HasData(tag1, tag2, tag3);

        // Seed NewsItems
        var news1 = new NewsItem(
            "Новость 1",
            "Краткое описание новости 1",
            "Полный контент новости 1",
            null
        );

        var news2 = new NewsItem(
            "Новость 2",
            "Краткое описание новости 2",
            "Полный контент новости 2",
            null
        );

        modelBuilder.Entity<NewsItem>().HasData(
            new
            {
                news1.Id,
                news1.Title,
                news1.ShortDescription,
                news1.Content,
                news1.ImageUrl,
                news1.CreatedAt,
                news1.UpdatedAt,
                news1.IsPublished,
                news1.ViewCount
            },
            new
            {
                news2.Id,
                news2.Title,
                news2.ShortDescription,
                news2.Content,
                news2.ImageUrl,
                news2.CreatedAt,
                news2.UpdatedAt,
                news2.IsPublished,
                news2.ViewCount
            }
        );

        // Связь Many-to-Many (NewsItem <-> Tag)
        modelBuilder.Entity("NewsItemTag").HasData(
            new { NewsItemsId = news1.Id, TagsId = tag1.Id },
            new { NewsItemsId = news1.Id, TagsId = tag2.Id },
            new { NewsItemsId = news2.Id, TagsId = tag3.Id }
        );
    }
}
