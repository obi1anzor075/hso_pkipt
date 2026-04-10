using System.ComponentModel.DataAnnotations;

namespace HsoPkipt.Models;

public class MerchItem
{
    public Guid Id { get; private set; }

    [Required]
    public string Name { get; private set; }

    public string? Description { get; private set; }

    [Range(0, 1000000)]
    public decimal Price { get; private set; }

    public string? ImageUrl { get; private set; }

    // связь с категорией
    public Guid TagId { get; private set; }
    public Tag Tag { get; private set; }

    protected MerchItem() { }

    public MerchItem(string name, decimal price, Guid tagId)
    {
        Id = Guid.NewGuid();
        Update(name, price, tagId, null, null);
    }

    public void Update(string name, decimal price, Guid tagId, string? description, string? imageUrl)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentNullException(nameof(name));

        if (price < 0)
            throw new ArgumentException("Цена не может быть отрицательной");

        Name = name;
        Price = price;
        TagId = tagId;
        Description = description;
        ImageUrl = imageUrl;
    }
}