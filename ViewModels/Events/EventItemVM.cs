namespace HsoPkipt.ViewModels.Events;

public class EventItemVM
{
    public Guid Id { get; set; }
    public string Title { get; set; }
    public DateTime EventDate { get; set; }
    public bool IsPublished { get; set; }
}