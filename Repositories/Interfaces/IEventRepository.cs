using HsoPkipt.Models;

namespace HsoPkipt.Repositories.Interfaces;

public interface IEventRepository
{
    Task<List<Event>> GetAllAsync();
    Task<Event?> GetByIdAsync(Guid id);
}