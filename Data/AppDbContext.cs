using Microsoft.EntityFrameworkCore;

namespace HsoPkipt.Models
{
    public class AppDbContext : DbContext
    {
        public DbSet<NewsItem> News { get; set; }
        public DbSet<Tag> Tags { get; set; }

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // ---- Сидируем Теги ----
            var tag1Id = Guid.Parse("11111111-1111-1111-1111-111111111111");
            var tag2Id = Guid.Parse("22222222-2222-2222-2222-222222222222");

            modelBuilder.Entity<Tag>().HasData(
                new { Id = tag1Id, Name = "Технологии" },
                new { Id = tag2Id, Name = "Игры" }
            );

            // ---- Сидируем Новости ----
            var baseDate = new DateTime(2026, 1, 29, 12, 0, 0, DateTimeKind.Utc);

            var newsIds = new[]
            {
                Guid.Parse("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaa1"),
                Guid.Parse("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaa2"),
                Guid.Parse("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaa3"),
                Guid.Parse("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaa4"),
                Guid.Parse("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaa5"),
                Guid.Parse("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaa6"),
                Guid.Parse("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaa7"),
                Guid.Parse("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaa8"),
                Guid.Parse("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaa9"),
                Guid.Parse("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaa0a"),
                Guid.Parse("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaa0b"),
                Guid.Parse("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaa0c"),
                Guid.Parse("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaa0d"),
                Guid.Parse("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaa0e"),
                Guid.Parse("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaa0f")
            };

            modelBuilder.Entity<NewsItem>().HasData(
                newsIds.Select((id, i) => new
                {
                    Id = id,
                    Title = $"Новость #{i + 1}",
                    ShortDescription = "Краткое описание",
                    Content = "Полное содержание",
                    ImageUrl = $"/assets/img/foto_1.png",
                    CreatedAt = baseDate.AddDays(-i),
                    UpdatedAt = baseDate.AddDays(-i),
                    IsPublished = true,
                    ViewCount = 0
                }).ToArray()
            );

            // --- Many-to-Many конфигурация и сидинг ---
            modelBuilder.Entity<NewsItem>()
                .HasMany(n => n.Tags)
                .WithMany(t => t.News)
                .UsingEntity<Dictionary<string, object>>(
                    "NewsItemTag",
                    j => j.HasOne<Tag>().WithMany().HasForeignKey("TagId"),
                    j => j.HasOne<NewsItem>().WithMany().HasForeignKey("NewsItemId"),
                    j =>
                    {
                        j.HasKey("NewsItemId", "TagId");

                        j.HasData(
                            // Игры
                            new { NewsItemId = newsIds[0], TagId = tag2Id },
                            new { NewsItemId = newsIds[1], TagId = tag2Id },
                            new { NewsItemId = newsIds[2], TagId = tag2Id },

                            // Технологии
                            new { NewsItemId = newsIds[3], TagId = tag1Id },
                            new { NewsItemId = newsIds[4], TagId = tag1Id },
                            new { NewsItemId = newsIds[5], TagId = tag1Id }
                        );
                    }
                );
        }
    }
}