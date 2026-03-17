using HsoPkipt.Services.Interfaces;
using HsoPkipt.ViewModels.Project;
using Microsoft.AspNetCore.Mvc;

namespace HsoPkipt.Controllers;

public class ProjectsController : Controller
{
    private readonly IProjectService _projectService;
    private const int PageSize = 8;

    public ProjectsController(IProjectService projectService)
    {
        _projectService = projectService;
    }

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

    [HttpGet]
    public async Task<IActionResult> LoadMoreProjects(int page, int pageSize)
    {
        var result = await _projectService.GetProjectPageAsync(page, pageSize);

        return PartialView("_ProjectsCardsPartial", result.Items);
    }

    [HttpGet]
    public async Task<IActionResult> Details(Guid id)
    {
        var project = await _projectService.GetByIdAsync(id);

        if (project == null)
            return NotFound();

        return View(project);
    }
}
