using HsoPkipt.Identity;
using HsoPkipt.Services.Interfaces;
using HsoPkipt.ViewModels.News;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HsoPkipt.Controllers.Admin;

// Контроллер для управления новостями в админке.
[Authorize(Roles = Roles.Admin + "," + Roles.Moderator)]
public class AdminNewsController : Controller
{
    // Сервис новостей.
    private readonly INewsService _newsService;

    // Получаем сервис через конструктор.
    public AdminNewsController(INewsService newsService)
    {
        _newsService = newsService;
    }

    // Показывает список новостей.
    [HttpGet]
    public async Task<IActionResult> Index(int page = 1)
    {
        const int pageSize = 10;

        var result = await _newsService.GetNewsPageAsync(page, pageSize);

        return View(result);
    }

    // Открывает форму создания новости.
    [HttpGet]
    public IActionResult Create()
    {
        return View(new CreateNewsItemVM());
    }

    // Сохраняет новую новость.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(CreateNewsItemVM model)
    {
        if (!ModelState.IsValid)
            return View(model);

        await _newsService.CreateAsync(model);

        return RedirectToAction(nameof(Index));
    }

    // Открывает форму редактирования новости.
    [HttpGet]
    public async Task<IActionResult> Edit(Guid id)
    {
        var news = await _newsService.GetForUpdateAsync(id);

        if (news == null)
            return NotFound();

        var model = new UpdateNewsItemVM
        {
            Title = news.Title,
            ShortDescription = news.ShortDescription,
            Content = news.Content,
            ImageUrl = news.ImageUrl,
            IsPublished = news.IsPublished
        };

        return View(model);
    }

    // Сохраняет изменения новости.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(Guid id, UpdateNewsItemVM model)
    {
        if (!ModelState.IsValid)
            return View(model);

        var updated = await _newsService.UpdateAsync(id, model);

        if (!updated)
            return NotFound();

        return RedirectToAction(nameof(Index));
    }

    // Показывает страницу удаления новости.
    [HttpGet]
    public async Task<IActionResult> Delete(Guid id)
    {
        var news = await _newsService.GetByIdAsync(id);

        if (news == null)
            return NotFound();

        return View(news);
    }

    // Удаляет новость после подтверждения.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(Guid id)
    {
        var deleted = await _newsService.DeleteAsync(id);

        if (!deleted)
            return NotFound();

        return RedirectToAction(nameof(Index));
    }
}
