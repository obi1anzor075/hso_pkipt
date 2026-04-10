using System.ComponentModel.DataAnnotations;

namespace HsoPkipt.Models;

public class Order
{
    public Guid Id { get; private set; }

    public Guid UserId { get; private set; }

    [Required]
    public string RecipientName { get; private set; } = string.Empty;

    [Required]
    public string PhoneNumber { get; private set; } = string.Empty;

    [Required]
    public string DeliveryAddress { get; private set; } = string.Empty;

    public DateTime CreatedAt { get; private set; }

    [Range(0, 1000000)]
    public decimal TotalPrice { get; private set; }

    public List<OrderItem> Items { get; private set; } = [];

    protected Order()
    {
    }

    public Order(Guid userId, string recipientName, string phoneNumber, string deliveryAddress, List<OrderItem> items)
    {
        if (userId == Guid.Empty)
            throw new ArgumentException("Пользователь не задан", nameof(userId));

        if (items.Count == 0)
            throw new ArgumentException("Заказ не может быть пустым", nameof(items));

        Id = Guid.NewGuid();
        UserId = userId;
        CreatedAt = DateTime.UtcNow;
        Items = items;

        UpdateContactInfo(recipientName, phoneNumber, deliveryAddress);
        TotalPrice = items.Sum(x => x.TotalPrice);
    }

    public void UpdateContactInfo(string recipientName, string phoneNumber, string deliveryAddress)
    {
        if (string.IsNullOrWhiteSpace(recipientName))
            throw new ArgumentNullException(nameof(recipientName));

        if (string.IsNullOrWhiteSpace(phoneNumber))
            throw new ArgumentNullException(nameof(phoneNumber));

        if (string.IsNullOrWhiteSpace(deliveryAddress))
            throw new ArgumentNullException(nameof(deliveryAddress));

        RecipientName = recipientName;
        PhoneNumber = phoneNumber;
        DeliveryAddress = deliveryAddress;
    }
}
