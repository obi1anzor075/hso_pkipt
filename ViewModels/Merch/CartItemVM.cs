namespace HsoPkipt.ViewModels.Merch;

public class CartItemVM
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public string? ImageUrl { get; set; }
    public int Quantity { get; set; }
    public decimal TotalPrice => Price * Quantity;
}
