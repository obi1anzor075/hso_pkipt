using System.ComponentModel.DataAnnotations;

namespace HsoPkipt.Models;

public class Event
{
    public Guid Id { get; set; }

    [Required]
    [Display(Name = "Название события")]
    public string Title { get; set; }

    [Required]
    [Display(Name = "Дата события")]
    public DateTime EventDate { get; set; }

    [Display(Name = "Описание")]
    public string Description { get; set; }

    public bool IsPublished { get; set; }
}