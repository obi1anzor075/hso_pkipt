namespace HsoPkipt.Models;

public class Tag
{
    public Guid Id { get; set; }
    public string Name { get; set; }

    private readonly List<NewsItem> _news = new();
    public ICollection<NewsItem> News => _news.AsReadOnly();

    public ICollection<NewsItemTag> NewsLink { get; set; } = new List<NewsItemTag>();

    public Tag()
    {
        Id = Guid.NewGuid();
        Name = string.Empty;
    }

    public Tag(string name)
    {
        Id = Guid.NewGuid();
        Name = name ?? throw new ArgumentNullException(nameof(name));
    }

    public void UpdateTag(string name)
    {
        if (string.IsNullOrEmpty(name))
            throw new ArgumentNullException(nameof(name));

        Name = name;
    }

    public void AddNewsItem(NewsItem newsItem)
    {
        if (newsItem == null)
            throw new ArgumentNullException(nameof(newsItem));

        _news.Add(newsItem);
    }
}
