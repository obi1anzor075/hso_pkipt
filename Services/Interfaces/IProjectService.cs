using HsoPkipt.Common;
using HsoPkipt.ViewModels.Project;

namespace HsoPkipt.Services.Interfaces;

public interface IProjectService 
{
    Task<IReadOnlyList<ProjectItemVM>> GetLatestAsync(int count = 5);
    Task<PagedResult<ProjectItemVM>> GetProjectPageAsync(int pageNumber, int pageSize);
    Task<ProjectDetailsVM?> GetByIdAsync(Guid id);
    Task<UpdateProjectVM?> GetForUpdateAsync(Guid id);

    Task<Guid> CreateAsync(CreateProjectVM model);
    Task<bool> UpdateAsync(Guid id, UpdateProjectVM model);
    Task<bool> DeleteAsync(Guid id);
    Task<int> CountAsync();
}
