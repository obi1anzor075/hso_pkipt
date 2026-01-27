namespace HsoPkipt.ViewModels.News;

public class NewsItemVM
{
    public Guid Id { get; init; }

    public string Title { get; init; } = default!;

    public string ShortDescription { get; init; } = default!;

    public string Content { get; init; } = default!;

    public string? ImageUrl { get; init; }

    public DateTime CreatedAt { get; init; }

    public DateTime UpdatedAt { get; init; }

    public bool IsPublished { get; init; }

    public int ViewCount { get; init; }

    public IReadOnlyCollection<string> Tags { get; init; } = [];
}
