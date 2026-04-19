using HsoPkipt.ViewModels.Tags;

namespace HsoPkipt.Services.Interfaces;

public interface ITagService
{
    // Возвращает все теги.
    Task<List<TagListItemVM>> GetAllAsync();

    // Ищет тег по id.
    Task<TagDetailsVM?> GetByIdAsync(Guid id);

    // Создаёт новый тег.
    Task CreateAsync(TagCreateVM vm);

    // Обновляет тег.
    Task UpdateAsync(TagEditVM vm);

    // Удаляет тег.
    Task DeleteAsync(Guid id);
}
