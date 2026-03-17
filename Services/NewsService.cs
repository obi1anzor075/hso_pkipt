using HsoPkipt.Common;
using HsoPkipt.Mappers;
using HsoPkipt.Repositories.Interfaces;
using HsoPkipt.Services.Interfaces;
using HsoPkipt.ViewModels.News;

namespace HsoPkipt.Services;

public class NewsService : INewsService
{
    private readonly INewsRepository _newsRepository;
    private readonly ITagRepository _tagRepository;

    public NewsService(INewsRepository newsRepository, ITagRepository tagRepository)
    {
        _newsRepository = newsRepository;
        _tagRepository = tagRepository;
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
        if (pageNumber < 1) pageNumber = 1;
        if (pageSize < 1) pageSize = 10;

        var (items, count) = await _newsRepository.GetPagedAsync(pageNumber, pageSize);

        var itemsVM = items.Select(i => i.ToViewModel()).ToList();

        return new PagedResult<NewsItemVM>(itemsVM, count, pageNumber, pageSize);
    }

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
            IsPublished = news.IsPublished,
            TagIds = news.Tags.Select(t => t.Id).ToList()
        };
    }

    public async Task<Guid> CreateAsync(CreateNewsItemVM model)
    {
        if (model is null)
            throw new ArgumentNullException(nameof(model));

        var newsItem = new Models.NewsItem(
            model.Title,
            model.ShortDescription ?? string.Empty,
            model.Content,
            model.ImageUrl
        );

        if (model.TagIds is not null && model.TagIds.Any())
        {
            var tags = await _tagRepository.GetByIdsAsync(model.TagIds);

            foreach (var tag in tags)
                newsItem.AddTag(tag);
        }

        await _newsRepository.CreateAsync(newsItem);

        return newsItem.Id;
    }

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
            model.ImageUrl
        );

        newsItem.SetPublish(model.IsPublished);

        var currentTags = newsItem.Tags.ToList();
        foreach (var tag in currentTags)
            newsItem.RemoveTag(tag);

        if (model.TagIds is not null && model.TagIds.Any())
        {
            var newTags = await _tagRepository.GetByIdsAsync(model.TagIds);

            foreach (var tag in newTags)
                newsItem.AddTag(tag);
        }

        await _newsRepository.UpdateAsync(newsItem);

        return true;
    }

    public async Task<bool> DeleteAsync(Guid id)
    {
        var newsItem = await _newsRepository.GetByIdAsync(id);

        if (newsItem is null)
            return false;

        await _newsRepository.DeleteAsync(newsItem);

        return true;
    }

    public async Task<int> CountAsync()
    {
        return await _newsRepository.CountAsync();
    }
}