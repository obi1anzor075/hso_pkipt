using HsoPkipt.Common;
using HsoPkipt.Models;
using HsoPkipt.ViewModels.News;

namespace HsoPkipt.Services.Interfaces;

public interface INewsService
{
    Task<IReadOnlyList<NewsItemVM>> GetLatestAsync(int count = 5);
    Task<PagedResult<NewsItem>> GetNewsPageAsync(int pageNumber, int pageSize);
}
