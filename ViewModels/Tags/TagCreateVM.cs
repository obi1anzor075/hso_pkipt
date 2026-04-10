using System.ComponentModel.DataAnnotations;

namespace HsoPkipt.ViewModels.Tags;

public class TagCreateVM
{
    [Required(ErrorMessage = "Введите название тега")]
    public string Name { get; set; }
}