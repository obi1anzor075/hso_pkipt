using System.ComponentModel.DataAnnotations;

namespace HsoPkipt.Models;

// Эта модель хранит одну новость сайта.
public class NewsItem
{
    // Уникальный ключ новости.
    public Guid Id { get; private set; }

    // Заголовок, который видит пользователь в карточке и на странице новости.
    [Required]
    [MaxLength(200)]
    public string Title { get; private set; }

    // Короткий текст для списка новостей.
    [MaxLength(500)]
    public string ShortDescription { get; private set; } = string.Empty;

    // Полный текст новости.
    [Required]
    public string Content { get; private set; }

    // Ссылка на картинку, если она есть.
    public string? ImageUrl { get; private set; }

    // Когда новость была создана.
    public DateTime CreatedAt { get; private set; }

    // Когда новость обновляли в последний раз.
    public DateTime UpdatedAt { get; private set; }

    // Показывать новость на сайте или нет.
    public bool IsPublished { get; private set; }

    // Сколько раз новость открывали.
    public int ViewCount { get; private set; }

    // Пустой конструктор нужен Entity Framework.
    protected NewsItem() { }

    // Обычный конструктор, когда создаём новость руками.
    public NewsItem(string title, string shortDescription, string content, string? imageUrl)
    {
        Id = Guid.NewGuid();
        CreatedAt = DateTime.UtcNow;
        UpdatedAt = DateTime.UtcNow;
        ViewCount = 0;
        IsPublished = true;

        Update(title, shortDescription, content, imageUrl);
    }

    // Меняет основные данные новости.
    public void Update(string title, string shortDescription, string content, string? imageUrl)
    {
        if (string.IsNullOrWhiteSpace(title))
            throw new ArgumentException("Заголовок обязателен", nameof(title));

        if (string.IsNullOrWhiteSpace(content))
            throw new ArgumentException("Содержимое обязательно", nameof(content));

        Title = title;
        ShortDescription = shortDescription ?? string.Empty;
        Content = content;
        ImageUrl = imageUrl;
        UpdatedAt = DateTime.UtcNow;
    }

    // Включает или выключает публикацию.
    public void SetPublish(bool isPublished)
    {
        IsPublished = isPublished;
        UpdatedAt = DateTime.UtcNow;
    }

    // Увеличивает счётчик просмотров на один.
    public void IncrementViewCount()
    {
        ViewCount++;
    }
}
