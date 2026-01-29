namespace HsoPkipt.Models;

public class NewsItemTag
{
    public Guid NewsItemId { get; set; }
    public NewsItem NewsItem { get; set; } = null!;

    public Guid TagId { get; set; }
    public Tag Tag { get; set; } = null!;
}
