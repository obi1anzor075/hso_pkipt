namespace HsoPkipt.ViewModels.Merch;

public class MerchCatalogVM
{
    public List<MerchItemVM> Items { get; set; } = [];
    public MerchCartVM Cart { get; set; } = new();
    public string SearchQuery { get; set; } = string.Empty;
}
