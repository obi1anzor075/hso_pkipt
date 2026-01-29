using HsoPkipt.Models;
using HsoPkipt.ViewModels.Projects;

namespace HsoPkipt.Mappers;

public static class ProjectMapper
{
    public static ProjectItemVm ToViewModel(this ProjectItem item)
    {
        return new ProjectItemVm
        {
            Id = item.Id,
            Title = item.Title,
            ShortDescription = item.ShortDescription,
            ImageUrl = item.ImageUrl,
            CreatedAt = item.CreatedAt
        };
    }
}
