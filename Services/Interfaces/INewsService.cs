using HsoPkipt.Common;
using HsoPkipt.ViewModels.News;

namespace HsoPkipt.Services.Interfaces;

public interface INewsService
{
    Task<IReadOnlyList<NewsItemVM>> GetLatestAsync(int count = 5);
    Task<PagedResult<NewsItemVM>> GetNewsPageAsync(int pageNumber, int pageSize);
    Task<NewsDetailsVM?> GetByIdAsync(Guid id);
    Task<UpdateNewsItemVM?> GetForUpdateAsync(Guid id);

    Task<Guid> CreateAsync(CreateNewsItemVM model);
    Task<bool> UpdateAsync(Guid id, UpdateNewsItemVM model);
    Task<bool> DeleteAsync(Guid id);
    Task<int> CountAsync();
}
