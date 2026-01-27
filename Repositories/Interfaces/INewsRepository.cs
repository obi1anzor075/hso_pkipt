using HsoPkipt.Models;

namespace HsoPkipt.Repositories.Interfaces;

public interface INewsRepository
{
    Task<NewsItem> GetLatestAsync();
    Task<List<NewsItem>> GetRangeAsync(int start, int take);
    Task<NewsItem> GetByIdAsync(Guid id);
    Task UpdateAsync(NewsItem item);
    Task CreateAsync(NewsItem item);
    Task DeleteAsync(NewsItem item);
    Task<int> CountAsync();
}
