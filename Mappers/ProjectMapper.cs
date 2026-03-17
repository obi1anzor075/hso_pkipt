using HsoPkipt.Models;
using HsoPkipt.ViewModels.Project;

namespace HsoPkipt.Mappers;

public static class ProjectMapper
{
    public static ProjectItemVM ToViewModel(this ProjectItem item)
    {
        return new ProjectItemVM
        {
            Id = item.Id,
            Title = item.Title,
            ShortDescription = item.ShortDescription,
            ImageUrl = item.ImageUrl,
            CreatedAt = item.CreatedAt,
            IsPublished = item.IsPublished
        };
    }
}
