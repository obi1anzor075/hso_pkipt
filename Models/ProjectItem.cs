using System.ComponentModel.DataAnnotations;

namespace HsoPkipt.Models;

public class ProjectItem
{
    public Guid Id { get; private set; }

    [Required]
    [MaxLength(200)]
    public string Title { get; private set; }

    [MaxLength(500)]
    public string ShortDescription { get; private set; } = string.Empty;

    [Required]
    public string Content { get; private set; }

    public string? ImageUrl { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public DateTime UpdatedAt { get; private set; }
    public bool IsPublished { get; private set; }

    protected ProjectItem() { }

    public ProjectItem(string title, string shortDescription, string content, string? imageUrl)
    {
        Id = Guid.NewGuid();
        CreatedAt = DateTime.UtcNow;
        UpdatedAt = DateTime.UtcNow;
        IsPublished = true;

        Update(title, shortDescription, content, imageUrl, true);
    }

    public void Update(string title, string shortDescription, string content, string? imageUrl, bool isPublished)
    {
        if (string.IsNullOrWhiteSpace(title)) throw new ArgumentException("Заголовок обязателен", nameof(title));
        if (string.IsNullOrWhiteSpace(content)) throw new ArgumentException("Содержимое обязательно", nameof(content));

        Title = title;
        ShortDescription = shortDescription ?? string.Empty;
        Content = content;
        ImageUrl = imageUrl;
        UpdatedAt = DateTime.UtcNow;
        IsPublished = isPublished;
    }

    public void SetPublish(bool isPublished)
    {
        IsPublished = isPublished;
        UpdatedAt = DateTime.UtcNow;
    }
}
