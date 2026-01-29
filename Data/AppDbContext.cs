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
            var news1Id = Guid.Parse("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa");
            var news2Id = Guid.Parse("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaab");
            var news3Id = Guid.Parse("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaac");

            modelBuilder.Entity<NewsItem>().HasData(
                new
                {
                    Id = news1Id,
                    Title = "Новая игра вышла",
                    ShortDescription = "Краткое описание",
                    Content = "Полное содержание",
                    ImageUrl = "/assets/img/foto_1.png",
                    CreatedAt = new DateTime(2026, 1, 29, 12, 0, 0, DateTimeKind.Utc),
                    UpdatedAt = new DateTime(2026, 1, 29, 12, 0, 0, DateTimeKind.Utc),
                    IsPublished = true,
                    ViewCount = 0
                },
                new
                {
                    Id = news2Id,
                    Title = "Новая игра вышла 2", // Изменил название для различия
                    ShortDescription = "Краткое описание",
                    Content = "Полное содержание",
                    ImageUrl = "/assets/img/foto_2.png",
                    CreatedAt = new DateTime(2026, 1, 29, 12, 0, 0, DateTimeKind.Utc),
                    UpdatedAt = new DateTime(2026, 1, 29, 12, 0, 0, DateTimeKind.Utc),
                    IsPublished = true,
                    ViewCount = 0
                },
                new
                {
                    Id = news3Id,
                    Title = "Ошибка в новости",
                    ShortDescription = "Краткое описание",
                    Content = "Полное содержание",
                    ImageUrl = "/assets/img/err.best_news_home.png",
                    CreatedAt = new DateTime(2026, 1, 29, 12, 0, 0, DateTimeKind.Utc),
                    UpdatedAt = new DateTime(2026, 1, 29, 12, 0, 0, DateTimeKind.Utc),
                    IsPublished = true,
                    ViewCount = 0
                }
            );

            // --- Many-to-Many конфигурация и сидинг ---
            // Поскольку класса NewsItemTag нет, мы настраиваем связь через Fluent API
            modelBuilder.Entity<NewsItem>()
                .HasMany(n => n.Tags)
                .WithMany(t => t.News)
                .UsingEntity<Dictionary<string, object>>(
                    "NewsItemTag", // Имя скрытой таблицы в БД

                    // Настройка "правой" стороны (Tag)
                    j => j.HasOne<Tag>().WithMany().HasForeignKey("TagId"),

                    // Настройка "левой" стороны (NewsItem)
                    j => j.HasOne<NewsItem>().WithMany().HasForeignKey("NewsItemId"),

                    // Настройка самой таблицы связи и наполнение данными (Seed)
                    j =>
                    {
                        j.HasKey("NewsItemId", "TagId"); // Составной ключ

                        j.HasData(
                            // Здесь мы используем анонимные объекты.
                            // Имена свойств должны совпадать с HasForeignKey выше.
                            new { NewsItemId = news1Id, TagId = tag2Id }, // Связываем 1 новость с тегом "Игры"
                            new { NewsItemId = news1Id, TagId = tag1Id }  // Связываем 1 новость с тегом "Технологии" (для примера)
                        );
                    }
                );
        }
    }
}