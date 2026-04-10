using HsoPkipt.Models;
using HsoPkipt.ViewModels.Events;

namespace HsoPkipt.Mappers;

public static class EventMapper
{
    public static EventItemVM ToViewModel(this Event model)
    {
        return new EventItemVM
        {
            Id = model.Id,
            Title = model.Title,
            EventDate = model.EventDate,
            IsPublished = model.IsPublished
        };
    }
}