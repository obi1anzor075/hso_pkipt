using HsoPkipt.Identity;
using HsoPkipt.Services.Interfaces;
using HsoPkipt.ViewModels.Project;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HsoPkipt.Controllers.Admin;

// Контроллер нужен для управления проектами в админке.
[Authorize(Roles = Roles.Admin + "," + Roles.Moderator)]
public class AdminProjectsController : Controller
{
    // Сервис проектов.
    private readonly IProjectService _projectService;

    // Получаем сервис через конструктор.
    public AdminProjectsController(IProjectService projectService)
    {
        _projectService = projectService;
    }

    // Показывает список проектов.
    [HttpGet]
    public async Task<IActionResult> Index(int page = 1)
    {
        const int pageSize = 10;

        var result = await _projectService.GetProjectPageAsync(page, pageSize);

        return View(result);
    }

    // Открывает форму создания проекта.
    [HttpGet]
    public IActionResult Create()
    {
        return View(new CreateProjectVM());
    }

    // Сохраняет новый проект.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(CreateProjectVM model)
    {
        if (!ModelState.IsValid)
            return View(model);

        await _projectService.CreateAsync(model);

        return RedirectToAction(nameof(Index));
    }

    // Открывает форму редактирования проекта.
    [HttpGet]
    public async Task<IActionResult> Edit(Guid id)
    {
        var model = await _projectService.GetForUpdateAsync(id);

        if (model == null)
            return NotFound();

        return View(model);
    }

    // Сохраняет изменения проекта.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(Guid id, UpdateProjectVM model)
    {
        if (!ModelState.IsValid)
            return View(model);

        var updated = await _projectService.UpdateAsync(id, model);

        if (!updated)
            return NotFound();

        return RedirectToAction(nameof(Index));
    }

    // Показывает страницу удаления проекта.
    [HttpGet]
    public async Task<IActionResult> Delete(Guid id)
    {
        var project = await _projectService.GetByIdAsync(id);

        if (project == null)
            return NotFound();

        return View(project);
    }

    // Удаляет проект после подтверждения.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(Guid id)
    {
        var deleted = await _projectService.DeleteAsync(id);

        if (!deleted)
            return NotFound();

        return RedirectToAction(nameof(Index));
    }
}
