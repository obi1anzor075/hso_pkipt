using HsoPkipt.Common;
using HsoPkipt.Mappers;
using HsoPkipt.Repositories.Interfaces;
using HsoPkipt.Services.Interfaces;
using HsoPkipt.ViewModels.News;

namespace HsoPkipt.Services;

public class NewsService : INewsService
{
    // репозиторий новостей
    private readonly INewsRepository _newsRepository;

    // репозиторий тегов
    private readonly ITagRepository _tagRepository;

    // получаем зависимости для работы с новостями и тегами
    public NewsService(INewsRepository newsRepository, ITagRepository tagRepository)
    {
        _newsRepository = newsRepository;
        _tagRepository = tagRepository;
    }

    // возвращаем несколько последних новостей для главной страницы или блока
    public async Task<IReadOnlyList<NewsItemVM>> GetLatestAsync(int count = 5)
    {
        var latestNews = await _newsRepository.GetLatestAsync(count);

        // если новостей нет, возвращаем пустой список
        if (latestNews is null)
            return new List<NewsItemVM>();

        // переводим сущности в модель для отображения
        return latestNews.Select(ni => ni.ToViewModel()).ToList();
    }

    // собираем страницу новостей с учётом пагинации
    public async Task<PagedResult<NewsItemVM>> GetNewsPageAsync(int pageNumber, int pageSize)
    {
        // подстраховываемся от некорректных параметров
        if (pageNumber < 1) pageNumber = 1;
        if (pageSize < 1) pageSize = 10;

        var (items, count) = await _newsRepository.GetPagedAsync(pageNumber, pageSize);

        // готовим новости для отдачи во view
        var itemsVM = items.Select(i => i.ToViewModel()).ToList();

        return new PagedResult<NewsItemVM>(itemsVM, count, pageNumber, pageSize);
    }

    // получаем подробные данные одной новости
    public async Task<NewsDetailsVM?> GetByIdAsync(Guid id)
    {
        var news = await _newsRepository.GetByIdAsync(id);

        // если новости нет, сообщаем об этом выше по цепочке
        if (news is null)
            return null;

        // собираем модель для страницы деталей
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

    // получаем данные новости для формы редактирования
    public async Task<UpdateNewsItemVM?> GetForUpdateAsync(Guid id)
    {
        var news = await _newsRepository.GetByIdAsync(id);

        // если новость не нашли, форму заполнять нечем
        if (news is null)
            return null;

        // переносим текущие данные в модель редактирования
        return new UpdateNewsItemVM
        {
            Title = news.Title,
            ShortDescription = news.ShortDescription,
            Content = news.Content,
            ImageUrl = news.ImageUrl,
            IsPublished = news.IsPublished,
            TagIds = news.Tags.Select(t => t.Id).ToList()
        };
    }

    // создаём новую новость и привязываем к ней теги
    public async Task<Guid> CreateAsync(CreateNewsItemVM model)
    {
        // не даём создать новость без входной модели
        if (model is null)
            throw new ArgumentNullException(nameof(model));

        // создаём доменную сущность новости
        var newsItem = new Models.NewsItem(
            model.Title,
            model.ShortDescription ?? string.Empty,
            model.Content,
            model.ImageUrl
        );

        // если теги выбраны, загружаем их и добавляем к новости
        if (model.TagIds is not null && model.TagIds.Any())
        {
            var tags = await _tagRepository.GetByIdsAsync(model.TagIds);

            foreach (var tag in tags)
                newsItem.AddTag(tag);
        }

        await _newsRepository.CreateAsync(newsItem);

        return newsItem.Id;
    }

    // обновляем новость и полностью пересобираем её теги
    public async Task<bool> UpdateAsync(Guid id, UpdateNewsItemVM model)
    {
        // без модели нечего обновлять
        if (model is


null)
            throw new ArgumentNullException(nameof(model));

        var newsItem = await _newsRepository.GetByIdAsync(id);

        // если новость не найдена, вернуть false
        if (newsItem is null)
            return false;

        // обновляем основные поля новости
        newsItem.Update(
            model.Title,
            model.ShortDescription ?? string.Empty,
            model.Content,
            model.ImageUrl
        );

        newsItem.SetPublish(model.IsPublished);

        // сначала убираем старые теги
        var currentTags = newsItem.Tags.ToList();
        foreach (var tag in currentTags)
            newsItem.RemoveTag(tag);

        // потом добавляем новый набор тегов
        if (model.TagIds is not null && model.TagIds.Any())
        {
            var newTags = await _tagRepository.GetByIdsAsync(model.TagIds);

            foreach (var tag in newTags)
                newsItem.AddTag(tag);
        }

        await _newsRepository.UpdateAsync(newsItem);

        return true;
    }

    // удаляем новость по id
    public async Task<bool> DeleteAsync(Guid id)
    {
        var newsItem = await _newsRepository.GetByIdAsync(id);

        // если удалять нечего, возвращаем false
        if (newsItem is null)
            return false;

        await _newsRepository.DeleteAsync(newsItem);

        return true;
    }

    // возвращаем общее количество новостей
    public async Task<int> CountAsync()
    {
        return await _newsRepository.CountAsync();
    }
}