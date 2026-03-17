namespace HsoPkipt.ViewModels.Project;

public class ProjectItemVM
{
    public Guid Id { get; set; }
    public string Title { get; set; }
    public string ShortDescription { get; set; }
    public string? ImageUrl { get; set; }
    public DateTime CreatedAt { get; set; }
    public bool IsPublished { get; init; }
}
