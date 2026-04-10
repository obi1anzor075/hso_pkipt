namespace HsoPkipt.ViewModels.Profile;

public class OrderVM
{
    public Guid Id { get; set; }
    public DateTime CreatedAt { get; set; }
    public string RecipientName { get; set; } = string.Empty;
    public string PhoneNumber { get; set; } = string.Empty;
    public string DeliveryAddress { get; set; } = string.Empty;
    public decimal TotalPrice { get; set; }
    public List<OrderItemVM> Items { get; set; } = [];
}
