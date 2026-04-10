using System.ComponentModel.DataAnnotations;

namespace HsoPkipt.ViewModels.Tags;

public class TagEditVM
{
    public Guid Id { get; set; }

    [Required(ErrorMessage = "Введите название тега")]
    public string Name { get; set; } = string.Empty;
}
