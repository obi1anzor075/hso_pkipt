namespace HsoPkipt.ViewModels.Profile;

public class OrderItemVM
{
    public string Name { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public int Quantity { get; set; }
    public decimal TotalPrice { get; set; }
}
