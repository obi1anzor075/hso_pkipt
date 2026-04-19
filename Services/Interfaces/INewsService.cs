using HsoPkipt.Common;
using HsoPkipt.ViewModels.News;

namespace HsoPkipt.Services.Interfaces;

public interface INewsService
{
    // Возвращает несколько последних новостей.
    Task<IReadOnlyList<NewsItemVM>> GetLatestAsync(int count = 5);

    // Возвращает одну страницу новостей.
    Task<PagedResult<NewsItemVM>> GetNewsPageAsync(int pageNumber, int pageSize);

    // Ищет новость по id.
    Task<NewsDetailsVM?> GetByIdAsync(Guid id);

    // Готовит новость для формы редактирования.
    Task<UpdateNewsItemVM?> GetForUpdateAsync(Guid id);

    // Создаёт новую новость.
    Task<Guid> CreateAsync(CreateNewsItemVM model);

    // Обновляет новость.
    Task<bool> UpdateAsync(Guid id, UpdateNewsItemVM model);

    // Удаляет новость.
    Task<bool> DeleteAsync(Guid id);

    // Возвращает общее количество новостей.
    Task<int> CountAsync();
}
