using System.ComponentModel.DataAnnotations;

namespace HsoPkipt.Models;

public class NewsItem
{
    public Guid Id { get; set; }

    [Required(ErrorMessage = "Заголовок обязателен")]
    [MaxLength(200, ErrorMessage = "Заголовок не должен превышать 200 символов")]
    public string Title { get; set; }

    [MaxLength(500)]
    public string ShortDescription { get; set; }

    [Required]
    public string Content { get; set; }

    public string? ImageUrl { get; set; }

    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }

    public bool IsPublished { get; set; }

    public int ViewCount { get; set; }

    public ICollection<Tag> Tags { get; set; } = new List<Tag>();

    public ICollection<NewsItemTag> TagsLink { get; set; } = new List<NewsItemTag>();


    public NewsItem()
    {
        Id = Guid.NewGuid();
        Title = string.Empty;
        ShortDescription = string.Empty;
        Content = string.Empty;
        CreatedAt = DateTime.UtcNow;
        UpdatedAt = DateTime.UtcNow;
        IsPublished = false;
        ViewCount = 0;
    }

    public NewsItem(string title, string shortDescription, string content, string? imageUrl)
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

        Tags = new List<Tag>();
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
