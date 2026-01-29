using HsoPkipt.Common;
using HsoPkipt.Mappers;
using HsoPkipt.Repositories.Interfaces;
using HsoPkipt.Services.Interfaces;
using HsoPkipt.ViewModels.News;

namespace HsoPkipt.Services;

public class NewsService : INewsService
{
    private readonly INewsRepository _newsRepository;

    public NewsService(INewsRepository newsRepository)
    {
        _newsRepository = newsRepository;
    }
    public async Task<IReadOnlyList<NewsItemVM>> GetLatestAsync(int count = 5)
    {
        var latestNews = await _newsRepository.GetLatestAsync(count);

        if (latestNews is null)
            return new List<NewsItemVM>();

        return latestNews.Select(ni => ni.ToViewModel()).ToList();
    }

    public async Task<PagedResult<NewsItemVM>> GetNewsPageAsync(int pageNumber, int pageSize)
    {
        // Страница и ее размер не может быть меньше 1
        if (pageNumber < 1) pageNumber = 1;
        if (pageSize < 1) pageSize = 10;

        var (items, count) = await _newsRepository.GetPagedAsync(pageNumber, pageSize);

        var itemsVM = items.Select(i => i.ToViewModel()).ToList();

        return new PagedResult<NewsItemVM>(itemsVM, count, pageNumber, pageSize);
    }
}
