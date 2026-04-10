using System.ComponentModel.DataAnnotations;

namespace HsoPkipt.ViewModels.Events;

public class EventEditVM
{
    public Guid Id { get; set; }

    [Required(ErrorMessage = "Название обязательно")]
    public string Title { get; set; }

    [Required]
    public DateTime EventDate { get; set; }

    public string Description { get; set; }

    public bool IsPublished { get; set; }
}