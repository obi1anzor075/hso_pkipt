using HsoPkipt.Models;

namespace HsoPkipt.Repositories.Interfaces;

public interface ITagRepository
{
    // Ищет тег по id.
    Task<Tag?> GetByIdAsync(Guid id);

    // Возвращает теги по списку id.
    Task<List<Tag>> GetByIdsAsync(IEnumerable<Guid> ids);

    // Возвращает все теги.
    Task<List<Tag>> GetAllAsync();

    // Ищет тег по названию.
    Task<Tag?> GetByNameAsync(string name);

    // Сохраняет новый тег.
    Task CreateAsync(Tag tag);

    // Сохраняет изменения тега.
    Task UpdateAsync(Tag tag);

    // Удаляет тег.
    Task DeleteAsync(Tag tag);
}
