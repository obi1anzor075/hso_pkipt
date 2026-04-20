using HsoPkipt.Models;

namespace HsoPkipt.Repositories.Interfaces;

public interface IProjectRepository
{
    // Возвращает несколько последних проектов.
    Task<List<ProjectItem>> GetLatestAsync(int count = 1);

    // Возвращает часть списка проектов.
    Task<List<ProjectItem>> GetRangeAsync(int start, int take);

    // Ищет проект по id.
    Task<ProjectItem?> GetByIdAsync(Guid id);

    // Возвращает страницу проектов и общее количество.
    Task<(List<ProjectItem> items, int count)> GetPagedAsync(int pageNumber, int pageSize);

    // Сохраняет изменения проекта.
    Task UpdateAsync(ProjectItem item);

    // Сохраняет новый проект.
    Task CreateAsync(ProjectItem item);

    // Удаляет проект.
    Task DeleteAsync(ProjectItem item);

    // Возвращает общее количество проектов.
    Task<int> CountAsync();
}
