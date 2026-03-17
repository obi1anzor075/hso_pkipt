using HsoPkipt.Identity;
using HsoPkipt.Services.Interfaces;
using HsoPkipt.ViewModels.News;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HsoPkipt.Controllers.Admin;

[Authorize(Roles = Roles.Admin + "," + Roles.Moderator)]
public class AdminNewsController : Controller
{
    private readonly INewsService _newsService;

    public AdminNewsController(INewsService newsService)
    {
        _newsService = newsService;
    }

    [HttpGet]
    public async Task<IActionResult> Index(int page = 1)
    {
        const int pageSize = 10;

        var result = await _newsService.GetNewsPageAsync(page, pageSize);

        return View(result);
    }

    [HttpGet]
    public IActionResult Create()
    {
        return View(new CreateNewsItemVM());
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(CreateNewsItemVM model)
    {
        if (!ModelState.IsValid)
            return View(model);

        await _newsService.CreateAsync(model);

        return RedirectToAction(nameof(Index));
    }

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
            IsPublished = false
        };

        return View(model);
    }

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

    [HttpGet]
    public async Task<IActionResult> Delete(Guid id)
    {
        var news = await _newsService.GetByIdAsync(id);

        if (news == null)
            return NotFound();

        return View(news);
    }

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