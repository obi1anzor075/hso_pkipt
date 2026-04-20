using HsoPkipt.Common;
using HsoPkipt.ViewModels.Project;

namespace HsoPkipt.Services.Interfaces;

public interface IProjectService 
{
    // Возвращает несколько последних проектов.
    Task<IReadOnlyList<ProjectItemVM>> GetLatestAsync(int count = 5);

    // Возвращает одну страницу проектов.
    Task<PagedResult<ProjectItemVM>> GetProjectPageAsync(int pageNumber, int pageSize);

    // Ищет проект по id.
    Task<ProjectDetailsVM?> GetByIdAsync(Guid id);

    // Готовит проект для формы редактирования.
    Task<UpdateProjectVM?> GetForUpdateAsync(Guid id);

    // Создаёт новый проект.
    Task<Guid> CreateAsync(CreateProjectVM model);

    // Обновляет проект.
    Task<bool> UpdateAsync(Guid id, UpdateProjectVM model);

    // Удаляет проект.
    Task<bool> DeleteAsync(Guid id);

    // Возвращает общее количество проектов.
    Task<int> CountAsync();
}
