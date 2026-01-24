using System.ComponentModel.DataAnnotations;

namespace HsoPkipt.Models;

public class NewsItem
{
    public Guid Id { get; private set; }

    [Required(ErrorMessage = "Заголовок обязателен")]
    [MaxLength(200, ErrorMessage = "Заголовок не должен превышать 200 символов")]
    public string Title { get; private set; }

    [MaxLength(500)]
    public string ShortDescription { get; private set; }

    [Required]
    public string Content { get; private set; }

    public string? ImageUrl { get; private set; }

    public DateTime CreatedAt { get; private set; }
    public DateTime UpdatedAt { get; private set; }

    public bool IsPublished { get; private set; }

    public int ViewCount { get; private set; }

    public ICollection<Tag> Tags { get; private set; } = new List<Tag>();

    private NewsItem()
    {
        Title = string.Empty;
        ShortDescription = string.Empty;
        Content = string.Empty;
    }

    public NewsItem(
        string title,
        string shortDescription,
        string content,
        string? imageUrl)
    {
        if (string.IsNullOrWhiteSpace(title))
            throw new ArgumentException("Заголовок не может быть пустым", nameof(title));

        if (string.IsNullOrWhiteSpace(content))
            throw new ArgumentException("Контент не может быть пустым", nameof(content));

        Id = Guid.NewGuid();
        Title = title;
        ShortDescription = shortDescription ?? string.Empty;
        Content = content;
        ImageUrl = imageUrl;

        CreatedAt = DateTime.UtcNow;
        UpdatedAt = DateTime.UtcNow;

        IsPublished = false;
        ViewCount = 0;
    }

    public void Update(string title, string shortDescription, string content, string? imageUrl)
    {
        if (string.IsNullOrWhiteSpace(title))
            throw new ArgumentException("Заголовок не может быть пустым", nameof(title));

        if (string.IsNullOrWhiteSpace(content))
            throw new ArgumentException("Контент не может быть пустым", nameof(content));

        Title = title;
        ShortDescription = shortDescription ?? string.Empty;
        Content = content;
        ImageUrl = imageUrl;

        UpdatedAt = DateTime.UtcNow;
    }

    public void SetPublish(bool isPublished)
    {
        IsPublished = isPublished;
        UpdatedAt = DateTime.UtcNow;
    }

    public void IncrementViewCount()
    {
        ViewCount++;
    }
}