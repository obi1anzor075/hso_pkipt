using HsoPkipt.Models;

namespace HsoPkipt.Repositories.Interfaces;

public interface INewsRepository
{
    // Возвращает несколько последних новостей.
    Task<List<NewsItem>> GetLatestAsync(int count = 1);

    // Возвращает часть списка новостей.
    Task<List<NewsItem>> GetRangeAsync(int start, int take);

    // Ищет новость по id.
    Task<NewsItem?> GetByIdAsync(Guid id);

    // Возвращает страницу новостей и общее количество.
    Task<(List<NewsItem> items, int count)> GetPagedAsync(int pageNumber, int pageSize);

    // Сохраняет изменения новости.
    Task UpdateAsync(NewsItem item);

    // Сохраняет новую новость.
    Task CreateAsync(NewsItem item);

    // Удаляет новость.
    Task DeleteAsync(NewsItem item);

    // Возвращает общее количество новостей.
    Task<int> CountAsync();
}
