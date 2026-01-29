using HsoPkipt.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace HsoPkipt.Controllers;

public class ProjectsController : Controller
{
    private readonly IProjectService _projectService;
    private const int PageSize = 9;

    public ProjectsController(IProjectService projectService)
    {
        _projectService = projectService;
    }
    [HttpGet]
    public async Task<IActionResult> Index()
    {
        var projects = await _projectService.GetProjectPageAsync(1, PageSize);

        return View(projects);
    }

    [HttpGet]
    public async Task<IActionResult> LoadMoreProjects(int page, int pageSize)
    {
        var result = await _projectService.GetProjectPageAsync(page, pageSize);

        return View(result);
    }
}
