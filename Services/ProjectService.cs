using HsoPkipt.Common;
using HsoPkipt.Models;
using HsoPkipt.Repositories.Interfaces;
using HsoPkipt.Services.Interfaces;
using HsoPkipt.ViewModels.Project;

namespace HsoPkipt.Services;

// Сервис готовит проекты для сайта и админки.
public class ProjectService : IProjectService
{
    // Репозиторий проектов.
    private readonly IProjectRepository _projectRepository;

    // Получаем репозиторий через конструктор.
    public ProjectService(IProjectRepository projectRepository)
    {
        _projectRepository = projectRepository;
    }

    // Возвращает последние проекты для главной страницы.
    public async Task<IReadOnlyList<ProjectItemVM>> GetLatestAsync(int count = 5)
    {
        var latestProjects = await _projectRepository.GetLatestAsync(count);

        if (latestProjects is null)
            return new List<ProjectItemVM>();

        return latestProjects.Select(p => new ProjectItemVM
        {
            Id = p.Id,
            Title = p.Title,
            ShortDescription = p.ShortDescription,
            ImageUrl = p.ImageUrl,
            CreatedAt = p.CreatedAt,
            IsPublished = p.IsPublished
        }).ToList();
    }

    // Собирает одну страницу проектов.
    public async Task<PagedResult<ProjectItemVM>> GetProjectPageAsync(int pageNumber, int pageSize)
    {
        if (pageNumber < 1)
            pageNumber = 1;

        if (pageSize < 1)
            pageSize = 10;

        var (items, count) = await _projectRepository.GetPagedAsync(pageNumber, pageSize);

        var itemsVm = items.Select(p => new ProjectItemVM
        {
            Id = p.Id,
            Title = p.Title,
            ShortDescription = p.ShortDescription,
            ImageUrl = p.ImageUrl,
            CreatedAt = p.CreatedAt,
            IsPublished = p.IsPublished
        }).ToList();

        return new PagedResult<ProjectItemVM>(itemsVm, count, pageNumber, pageSize);
    }

    // Ищет один проект по id.
    public async Task<ProjectDetailsVM?> GetByIdAsync(Guid id)
    {
        var project = await _projectRepository.GetByIdAsync(id);

        if (project is null)
            return null;

        return new ProjectDetailsVM
        {
            Id = project.Id,
            Title = project.Title,
            ShortDescription = project.ShortDescription,
            Content = project.Content,
            ImageUrl = project.ImageUrl,
            CreatedAt = project.CreatedAt,
            UpdatedAt = project.UpdatedAt
        };
    }

    // Готовит проект для формы редактирования.
    public async Task<UpdateProjectVM?> GetForUpdateAsync(Guid id)
    {
        var project = await _projectRepository.GetByIdAsync(id);

        if (project is null)
            return null;

        return new UpdateProjectVM
        {
            Title = project.Title,
            ShortDescription = project.ShortDescription,
            Content = project.Content,
            ImageUrl = project.ImageUrl,
            IsPublished = project.IsPublished
        };
    }

    // Создаёт новый проект.
    public async Task<Guid> CreateAsync(CreateProjectVM model)
    {
        if (model is null)
            throw new ArgumentNullException(nameof(model));

        var project = new ProjectItem(
            model.Title,
            model.ShortDescription ?? string.Empty,
            model.Content,
            model.ImageUrl);

        await _projectRepository.CreateAsync(project);

        return project.Id;
    }

    // Обновляет существующий проект.
    public async Task<bool> UpdateAsync(Guid id, UpdateProjectVM model)
    {
        if (model is null)
            throw new ArgumentNullException(nameof(model));

        var project = await _projectRepository.GetByIdAsync(id);

        if (project is null)
            return false;

        project.Update(
            model.Title,
            model.ShortDescription ?? string.Empty,
            model.Content,
            model.ImageUrl,
            model.IsPublished);

        project.SetPublish(model.IsPublished);

        await _projectRepository.UpdateAsync(project);

        return true;
    }

    // Удаляет проект.
    public async Task<bool> DeleteAsync(Guid id)
    {
        var project = await _projectRepository.GetByIdAsync(id);

        if (project is null)
            return false;

        await _projectRepository.DeleteAsync(project);

        return true;
    }

    // Возвращает только число проектов.
    public async Task<int> CountAsync()
    {
        return await _projectRepository.CountAsync();
    }
}
