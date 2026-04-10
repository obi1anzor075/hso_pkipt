using System.ComponentModel.DataAnnotations;

namespace HsoPkipt.ViewModels.Merch;

public class UpdateMerchItemVM
{
    [Required]
    public string Name { get; set; } = string.Empty;

    public string? Description { get; set; }

    [Required]
    public decimal Price { get; set; }

    public string? ImageUrl { get; set; }

    [Required]
    public Guid TagId { get; set; }
}
