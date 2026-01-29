using HsoPkipt.Common;
using HsoPkipt.ViewModels.Projects;

namespace HsoPkipt.Services.Interfaces;

public interface IProjectService 
{
    Task<IReadOnlyList<ProjectItemVm>> GetLatestAsync(int count = 5);
    Task<PagedResult<ProjectItemVm>> GetProjectPageAsync(int pageNumber, int pageSize);
}
