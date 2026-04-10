using HsoPkipt.Models;

namespace HsoPkipt.Repositories.Interfaces;

public interface IEventRepository
{
    Task<List<Event>> GetAllAsync();
    Task<Event?> GetByIdAsync(Guid id);

    Task AddAsync(Event entity);
    Task UpdateAsync(Event entity);
    Task DeleteAsync(Event entity);
    Task<(List<Event> items, int count)> GetPagedAsync(int pageNumber, int pageSize);
}