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
}
