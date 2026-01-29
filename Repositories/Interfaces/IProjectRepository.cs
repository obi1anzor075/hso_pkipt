using HsoPkipt.Models;

namespace HsoPkipt.Repositories.Interfaces;

public interface IProjectRepository
{
    Task<List<ProjectItem>> GetLatestAsync(int count = 1);
    Task<List<ProjectItem>> GetRangeAsync(int start, int take);
    Task<ProjectItem?> GetByIdAsync(Guid id);
    Task<(List<ProjectItem> items, int count)> GetPagedAsync(int pageNumber, int pageSize);
    Task UpdateAsync(ProjectItem item);
    Task CreateAsync(ProjectItem item);
    Task DeleteAsync(ProjectItem item);
    Task<int> CountAsync();
}
