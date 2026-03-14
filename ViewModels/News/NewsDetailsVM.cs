namespace HsoPkipt.ViewModels.News;

public class NewsDetailsVM
{
    public Guid Id { get; set; }

    public string Title { get; set; } = "";

    public string ShortDescription { get; set; } = "";

    public string Content { get; set; } = "";

    public string? ImageUrl { get; set; }

    public DateTime CreatedAt { get; set; }
}