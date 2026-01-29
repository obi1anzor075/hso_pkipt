using HsoPkipt.Models;

namespace HsoPkipt.Repositories.Interfaces;

public interface INewsRepository
{
    Task<List<NewsItem>> GetLatestAsync(int count = 1);
    Task<List<NewsItem>> GetRangeAsync(int start, int take);
    Task<NewsItem?> GetByIdAsync(Guid id);
    Task<(List<NewsItem> items, int count)> GetPagedAsync(int pageNumber, int pageSize);
    Task UpdateAsync(NewsItem item);
    Task CreateAsync(NewsItem item);
    Task DeleteAsync(NewsItem item);
    Task<int> CountAsync();
}
