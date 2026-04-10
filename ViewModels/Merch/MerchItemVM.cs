namespace HsoPkipt.ViewModels.Merch;

public class MerchItemVM
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public decimal Price { get; set; }
    public string? ImageUrl { get; set; }
    public Guid TagId { get; set; }
    public string? TagName { get; set; }
}
