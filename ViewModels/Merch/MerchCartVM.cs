namespace HsoPkipt.ViewModels.Merch;

public class MerchCartVM
{
    public List<CartItemVM> Items { get; set; } = [];
    public int ItemsCount { get; set; }
    public decimal TotalPrice { get; set; }
    public bool IsEmpty => ItemsCount == 0;
}
