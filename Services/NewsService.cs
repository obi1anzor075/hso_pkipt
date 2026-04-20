using HsoPkipt.Common;
using HsoPkipt.Mappers;
using HsoPkipt.Repositories.Interfaces;
using HsoPkipt.Services.Interfaces;
using HsoPkipt.ViewModels.News;

namespace HsoPkipt.Services;

// Сервис собирает данные по новостям и отдаёт их в удобном виде для контроллеров.
public class NewsService : INewsService
{
    // Репозиторий нужен для чтения и сохранения новостей.
    private readonly INewsRepository _newsRepository;

    // Получаем репозиторий через конструктор.
    public NewsService(INewsRepository newsRepository)
    {
        _newsRepository = newsRepository;
    }

    // Возвращает последние новости для главной страницы.
    public async Task<IReadOnlyList<NewsItemVM>> GetLatestAsync(int count = 5)
    {
        var latestNews = await _newsRepository.GetLatestAsync(count);

        if (latestNews is null)
            return new List<NewsItemVM>();

        return latestNews.Select(ni => ni.ToViewModel()).ToList();
    }

    // Собирает одну страницу новостей.
    public async Task<PagedResult<NewsItemVM>> GetNewsPageAsync(int pageNumber, int pageSize)
    {
        if (pageNumber < 1)
            pageNumber = 1;

        if (pageSize < 1)
            pageSize = 10;

        var (items, count) = await _newsRepository.GetPagedAsync(pageNumber, pageSize);
        var itemsVm = items.Select(i => i.ToViewModel()).ToList();

        return new PagedResult<NewsItemVM>(itemsVm, count, pageNumber, pageSize);
    }

    // Возвращает полную информацию по одной новости.
    public async Task<NewsDetailsVM?> GetByIdAsync(Guid id)
    {
        var news = await _newsRepository.GetByIdAsync(id);

        if (news is null)
            return null;

        return new NewsDetailsVM
        {
            Id = news.Id,
            Title = news.Title,
            Content = news.Content,
            ShortDescription = news.ShortDescription,
            ImageUrl = news.ImageUrl,
            CreatedAt = news.CreatedAt
        };
    }

    // Готовит модель для формы редактирования.
    public async Task<UpdateNewsItemVM?> GetForUpdateAsync(Guid id)
    {
        var news = await _newsRepository.GetByIdAsync(id);

        if (news is null)
            return null;

        return new UpdateNewsItemVM
        {
            Title = news.Title,
            ShortDescription = news.ShortDescription,
            Content = news.Content,
            ImageUrl = news.ImageUrl,
            IsPublished = news.IsPublished
        };
    }

    // Создаёт новую новость и возвращает её id.
    public async Task<Guid> CreateAsync(CreateNewsItemVM model)
    {
        if (model is null)
            throw new ArgumentNullException(nameof(model));

        var newsItem = new Models.NewsItem(
            model.Title,
            model.ShortDescription ?? string.Empty,
            model.Content,
            model.ImageUrl);

        newsItem.SetPublish(model.IsPublished);

        await _newsRepository.CreateAsync(newsItem);

        return newsItem.Id;
    }

    // Обновляет существующую новость.
    public async Task<bool> UpdateAsync(Guid id, UpdateNewsItemVM model)
    {
        if (model is null)
            throw new ArgumentNullException(nameof(model));

        var newsItem = await _newsRepository.GetByIdAsync(id);

        if (newsItem is null)
            return false;

        newsItem.Update(
            model.Title,
            model.ShortDescription ?? string.Empty,
            model.Content,
            model.ImageUrl);

        newsItem.SetPublish(model.IsPublished);

        await _newsRepository.UpdateAsync(newsItem);

        return true;
    }

    // Удаляет новость по id.
    public async Task<bool> DeleteAsync(Guid id)
    {
        var newsItem = await _newsRepository.GetByIdAsync(id);

        if (newsItem is null)
            return false;

        await _newsRepository.DeleteAsync(newsItem);

        return true;
    }

    // Возвращает число новостей без списка.
    public async Task<int> CountAsync()
    {
        return await _newsRepository.CountAsync();
    }
}
