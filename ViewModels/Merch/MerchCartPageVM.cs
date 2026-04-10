namespace HsoPkipt.ViewModels.Merch;

public class MerchCartPageVM
{
    public MerchCartVM Cart { get; set; } = new();
    public CheckoutOrderVM Checkout { get; set; } = new();
    public bool IsAuthenticated { get; set; }
}
