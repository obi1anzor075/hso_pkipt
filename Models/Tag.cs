namespace HsoPkipt.Models;

public class Tag
{
    public Guid Id { get; private set; }

    public string Name { get; private set; }

    private readonly List<NewsItem> _news = new();
    public ICollection<NewsItem> News => _news.AsReadOnly();

    private Tag()
    {
        Name = string.Empty;
    }

    public Tag(string name)
    {
        Id = Guid.NewGuid();
        Name = name;
    }

    public void UpdateTag(string name)
    {
        if (string.IsNullOrEmpty(name))
            throw new ArgumentNullException(nameof(name));

        Name = name;
    }
}
