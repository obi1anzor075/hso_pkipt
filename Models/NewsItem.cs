using System.ComponentModel.DataAnnotations;

namespace HsoPkipt.Models;

public class NewsItem
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
    public int ViewCount { get; private set; }

    // Связь многие-ко-многим
    public virtual ICollection<Tag> Tags { get; private set; } = new List<Tag>();

    protected NewsItem() { }

    public NewsItem(string title, string shortDescription, string content, string? imageUrl)
    {
        Id = Guid.NewGuid();
        CreatedAt = DateTime.UtcNow;
        UpdatedAt = DateTime.UtcNow;
        ViewCount = 0;
        IsPublished = false;

        Update(title, shortDescription, content, imageUrl);
    }

    public void Update(string title, string shortDescription, string content, string? imageUrl)
    {
        if (string.IsNullOrWhiteSpace(title)) throw new ArgumentException("Заголовок обязателен", nameof(title));
        if (string.IsNullOrWhiteSpace(content)) throw new ArgumentException("Содержимоле обязательно", nameof(content));

        Title = title;
        ShortDescription = shortDescription ?? string.Empty;
        Content = content;
        ImageUrl = imageUrl;
        UpdatedAt = DateTime.UtcNow;
    }

    public void AddTag(Tag tag)
    {
        if (tag == null) return;
        if (!Tags.Contains(tag)) Tags.Add(tag);
    }

    public void RemoveTag(Tag tag)
    {
        if (Tags.Contains(tag)) Tags.Remove(tag);
    }

    public void SetPublish(bool isPublished)
    {
        IsPublished = isPublished;
        UpdatedAt = DateTime.UtcNow;
    }

    public void IncrementViewCount() => ViewCount++;
}