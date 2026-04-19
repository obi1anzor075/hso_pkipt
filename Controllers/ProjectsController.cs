using HsoPkipt.Services.Interfaces;
using HsoPkipt.ViewModels.Project;
using Microsoft.AspNetCore.Mvc;

namespace HsoPkipt.Controllers;

// Контроллер показывает список проектов и страницу одного проекта.
public class ProjectsController : Controller
{
    // Сервис проектов.
    private readonly IProjectService _projectService;

    // Размер одной страницы со списком проектов.
    private const int PageSize = 8;

    // Получаем сервис через конструктор.
    public ProjectsController(IProjectService projectService)
    {
        _projectService = projectService;
    }

    // Показывает первую страницу проектов.
    [HttpGet]
    public async Task<IActionResult> Index()
    {
        var result = await _projectService.GetProjectPageAsync(1, PageSize);

        return View(new ProjectsVM
        {
            ProjectItems = result.Items,
            PageNumber = result.CurrentPage,
            TotalPages = result.TotalPages
        });
    }

    // Догружает ещё проекты для списка.
    [HttpGet]
    public async Task<IActionResult> LoadMoreProjects(int page, int pageSize)
    {
        var result = await _projectService.GetProjectPageAsync(page, pageSize);

        return PartialView("_ProjectsCardsPartial", result.Items);
    }

    // Показывает подробную страницу одного проекта.
    [HttpGet]
    public async Task<IActionResult> Details(Guid id)
    {
        var project = await _projectService.GetByIdAsync(id);

        if (project == null)
            return NotFound();

        return View(project);
    }
}
