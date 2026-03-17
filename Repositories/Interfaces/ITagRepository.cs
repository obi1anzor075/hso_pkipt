using HsoPkipt.Models;

namespace HsoPkipt.Repositories.Interfaces;

public interface ITagRepository
{
    Task<Tag?> GetByIdAsync(Guid id);
    Task<List<Tag>> GetByIdsAsync(IEnumerable<Guid> ids);
    Task<List<Tag>> GetAllAsync();
    Task<Tag?> GetByNameAsync(string name);
    Task CreateAsync(Tag tag);
    Task UpdateAsync(Tag tag);
    Task DeleteAsync(Tag tag);
}