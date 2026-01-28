using HsoPkipt.ViewModels.News;

namespace HsoPkipt.Services.Interfaces;

public interface INewsService
{
    Task<IReadOnlyList<NewsItemVM>> GetLatestAsync(int count = 5);
}
