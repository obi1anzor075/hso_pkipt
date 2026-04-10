using System.ComponentModel.DataAnnotations;

namespace HsoPkipt.ViewModels.Events;

public class EventCreateVM
{
    [Required (ErrorMessage = "Название обязательно")]
    public string Title { get; set; }

    [Required]
    public DateTime EventDate { get; set; }

    public string Description { get; set; }
}