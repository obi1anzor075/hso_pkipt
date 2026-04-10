namespace HsoPkipt.ViewModels.Events;

public class EventDetailsVM
{
    public Guid Id { get; set; }
    public string Title { get; set; }
    public DateTime EventDate { get; set; }
    public string Description { get; set; }
    public bool IsPublished { get; set; }
}