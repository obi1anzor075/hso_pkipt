namespace HsoPkipt.ViewModels.News;

public class NewsVM
{
    public IEnumerable<NewsItemVM> NewsItems { get; set; } = Enumerable.Empty<NewsItemVM>();

    public int PageNumber { get; set; }
    public int TotalPages { get; set; }

    public bool HasNextPage => PageNumber < TotalPages;
}
