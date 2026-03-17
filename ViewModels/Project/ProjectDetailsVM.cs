namespace HsoPkipt.ViewModels.Project;

public class ProjectDetailsVM
{
    public Guid Id { get; init; }
    public string Title { get; init; } = default!;
    public string? ShortDescription { get; init; }
    public string Content { get; init; } = default!;
    public string? ImageUrl { get; init; }
    public DateTime CreatedAt { get; init; }
    public DateTime UpdatedAt { get; init; }
}