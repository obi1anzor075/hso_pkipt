using System.ComponentModel.DataAnnotations;

namespace HsoPkipt.Models;

public class Tag
{
    public Guid Id { get; private set; }

    [Required]
    public string Name { get; private set; }

    public virtual ICollection<NewsItem> News { get; private set; } = new List<NewsItem>();

    protected Tag() { }

    public Tag(string name)
    {
        Id = Guid.NewGuid();
        UpdateTag(name);
    }

    public void UpdateTag(string name)
    {
        if (string.IsNullOrWhiteSpace(name)) throw new ArgumentNullException(nameof(name));
        Name = name;
    }
}