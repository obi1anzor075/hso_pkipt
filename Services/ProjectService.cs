using HsoPkipt.Common;
using HsoPkipt.Mappers;

using HsoPkipt.Repositories.Interfaces;
using HsoPkipt.Services.Interfaces;
using HsoPkipt.ViewModels.Projects;

namespace HsoPkipt.Services;

public class ProjectService : IProjectService
{
    private readonly IProjectRepository _projectRepository;

    public ProjectService(
        IProjectRepository projectRepository)
    {
        _projectRepository = projectRepository;
    }
    public async Task<IReadOnlyList<ProjectItemVm>> GetLatestAsync(int count = 5)
    {
        var latestProjects = await _projectRepository.GetLatestAsync(count);

        if (latestProjects is null)
            return new List<ProjectItemVm>();

        return latestProjects.Select(p => p.ToViewModel()).ToList();
    }

    public async Task<PagedResult<ProjectItemVm>> GetProjectPageAsync(int pageNumber, int pageSize)
    {
        // Страница и ее размер не может быть меньше 1
        if (pageNumber < 1) pageNumber = 1;
        if (pageSize < 1) pageSize = 10;

        var (items, count) = await _projectRepository.GetPagedAsync(pageNumber, pageSize);

        var itemsVM = items.Select(p => p.ToViewModel()).ToList();

        return new PagedResult<ProjectItemVm>(itemsVM, count, pageNumber, pageSize);
    }
}
