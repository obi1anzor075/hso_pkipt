using HsoPkipt.Common;
using HsoPkipt.ViewModels.Events;

namespace HsoPkipt.Services.Interfaces;

public interface IEventService
{
    Task<List<EventItemVM>> GetAllAsync();
    Task<EventDetailsVM?> GetByIdAsync(Guid id);

    Task CreateAsync(EventCreateVM vm);
    Task UpdateAsync(EventEditVM vm);
    Task DeleteAsync(Guid id);
    Task<PagedResult<EventItemVM>> GetPageAsync(int pageNumber, int pageSize);
}