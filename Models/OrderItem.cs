using System.ComponentModel.DataAnnotations;

namespace HsoPkipt.Models;

public class OrderItem
{
    public Guid Id { get; private set; }

    public Guid OrderId { get; private set; }
    public Order Order { get; private set; } = null!;

    public Guid MerchItemId { get; private set; }

    [Required]
    public string MerchItemName { get; private set; } = string.Empty;

    [Range(0, 1000000)]
    public decimal Price { get; private set; }

    [Range(1, 1000)]
    public int Quantity { get; private set; }

    public decimal TotalPrice => Price * Quantity;

    protected OrderItem()
    {
    }

    public OrderItem(Guid merchItemId, string merchItemName, decimal price, int quantity)
    {
        if (merchItemId == Guid.Empty)
            throw new ArgumentException("Товар не задан", nameof(merchItemId));

        if (string.IsNullOrWhiteSpace(merchItemName))
            throw new ArgumentNullException(nameof(merchItemName));

        if (price < 0)
            throw new ArgumentException("Цена не может быть отрицательной", nameof(price));

        if (quantity <= 0)
            throw new ArgumentException("Количество должно быть больше нуля", nameof(quantity));

        Id = Guid.NewGuid();
        MerchItemId = merchItemId;
        MerchItemName = merchItemName;
        Price = price;
        Quantity = quantity;
    }
}
