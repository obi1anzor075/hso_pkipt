using System.ComponentModel.DataAnnotations;

namespace HsoPkipt.ViewModels.Profile;

public class EventVM
{
    public Guid Id { get; set; }

    [Display(Name = "Название")]
    public string Title { get; set; } = "";

    [Display(Name = "Описание")]
    public string Description { get; set; } = "";

    [Display(Name = "Дата события")]
    public DateTime Date { get; set; }

    public bool IsPublished { get; set; }
}