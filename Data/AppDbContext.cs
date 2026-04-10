using HsoPkipt.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace HsoPkipt.Models
{
    public class AppDbContext : IdentityDbContext<AppUser, AppRole, Guid>
    {
        public DbSet<NewsItem> News { get; set; }
        public DbSet<ProjectItem> Projects { get; set; }
        public DbSet<Tag> Tags { get; set; }
        public DbSet<Event> Events { get; set; }
        public DbSet<MerchItem> MerchItems { get; set; }

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Event>()
                .Property(e => e.EventDate)
                .HasConversion(
                    v => v.ToUniversalTime(),
                    v => DateTime.SpecifyKind(v, DateTimeKind.Utc)
                );

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

            // ---- Сидируем Проекты ----
            var projectBaseDate = new DateTime(2026, 1, 29, 12, 0, 0, DateTimeKind.Utc);

            var projectIds = new[]
            {
                Guid.Parse("bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbb1"),
                Guid.Parse("bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbb2"),
                Guid.Parse("bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbb3"),
                Guid.Parse("bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbb4"),
                Guid.Parse("bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbb5"),
                Guid.Parse("bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbb6"),
                Guid.Parse("bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbb7"),
                Guid.Parse("bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbb8"),
                Guid.Parse("bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbb9"),
                Guid.Parse("bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbb0"),
                Guid.Parse("bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbbd"),
                Guid.Parse("bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbbe"),
                Guid.Parse("bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbbf"),
                Guid.Parse("bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbc1"),
                Guid.Parse("bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbc2")
            };

            modelBuilder.Entity<ProjectItem>().HasData(
                projectIds.Select((id, i) => new
                {
                    Id = id,
                    Title = $"Проект #{i + 1}",
                    ShortDescription = "Краткое описание проекта",
                    Content = "Подробное описание проекта. Используемые технологии, цели и результат.",
                    ImageUrl = $"/assets/img/foto_2.png",
                    CreatedAt = projectBaseDate.AddDays(-i),
                    UpdatedAt = projectBaseDate.AddDays(-i),
                    IsPublished = true
                }).ToArray()
            );

            // ---- Сидируем События ----
            var eventBaseDate = new DateTime(2026, 3, 20, 10, 0, 0, DateTimeKind.Utc);

            modelBuilder.Entity<Event>().HasData(
                new
                {
                    Id = Guid.Parse("cccccccc-cccc-cccc-cccc-ccccccccccc1"),
                    Title = "Открытие весенней смены",
                    Description = "Торжественное открытие весенней смены с участием всех отрядов.",
                    EventDate = eventBaseDate,
                    IsPublished = true
                },
                new
                {
                    Id = Guid.Parse("cccccccc-cccc-cccc-cccc-ccccccccccc2"),
                    Title = "Турнир по настольным играм",
                    Description = "Командное соревнование по настольным играм среди участников лагеря.",
                    EventDate = eventBaseDate.AddDays(2),
                    IsPublished = true
                },
                new
                {
                    Id = Guid.Parse("cccccccc-cccc-cccc-cccc-ccccccccccc3"),
                    Title = "Вечер талантов",
                    Description = "Творческий вечер с выступлениями участников и вожатых.",
                    EventDate = eventBaseDate.AddDays(5),
                    IsPublished = true
                }
            );

            modelBuilder.Entity<MerchItem>().HasData(
                new
                {
                    Id = Guid.Parse("dddddddd-dddd-dddd-dddd-ddddddddddd1"),
                    Name = "Футболка",
                    Description = "Фирменная футболка",
                    Price = 500m,
                    ImageUrl = "/assets/img/foto_3.png",
                    TagId = tag1Id
                },
                new
                {
                    Id = Guid.Parse("dddddddd-dddd-dddd-dddd-ddddddddddd2"),
                    Name = "Кепка",
                    Description = "Стильная кепка",
                    Price = 300m,
                    ImageUrl = "/assets/img/foto_3.png",
                    TagId = tag1Id
                },
                new
                {
                    Id = Guid.Parse("dddddddd-dddd-dddd-dddd-ddddddddddd3"),
                    Name = "Брелок",
                    Description = "Металлический брелок",
                    Price = 100m,
                    ImageUrl = "/assets/img/foto_3.png",
                    TagId = tag2Id
                }
            );
        }
    }
}