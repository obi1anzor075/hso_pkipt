using HsoPkipt.ViewModels.News;

namespace HsoPkipt.Services.Interfaces;

public interface INewsService
{
    Task<NewsItemVM> GetLatestAsync(int count = 5);
}
