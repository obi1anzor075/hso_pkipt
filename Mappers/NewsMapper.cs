using HsoPkipt.Models;
using HsoPkipt.ViewModels.News;

namespace HsoPkipt.Mappers;

public static class NewsMapper
{
    public static NewsItemVM ToViewModel(this NewsItem entity)
    {
        return new NewsItemVM
        {
            Id = entity.Id,
            Title = entity.Title,
            ShortDescription = entity.ShortDescription,
            ImageUrl = entity.ImageUrl,
            CreatedAt = entity.CreatedAt,
        };
    }
}
