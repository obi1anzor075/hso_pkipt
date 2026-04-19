using HsoPkipt.Identity;
using HsoPkipt.Services.Interfaces;
using HsoPkipt.ViewModels.Tags;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HsoPkipt.Controllers.Admin;

// Контроллер для управления категориями товаров.
[Authorize(Roles = Roles.Admin)]
public class AdminTagController : Controller
{
    // Сервис категорий.
    private readonly ITagService _tagService;

    // Получаем сервис через конструктор.
    public AdminTagController(ITagService tagService)
    {
        _tagService = tagService;
    }

    // Показывает список категорий.
    public async Task<IActionResult> Tags()
    {
        var tags = await _tagService.GetAllAsync();
        return View(tags);
    }

    // Открывает форму создания категории.
    [HttpGet]
    public IActionResult CreateTag()
    {
        return View(new TagCreateVM());
    }

    // Сохраняет новую категорию.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> CreateTag(TagCreateVM vm)
    {
        if (!ModelState.IsValid)
            return View(vm);

        try
        {
            await _tagService.CreateAsync(vm);
        }
        catch (Exception ex)
        {
            ModelState.AddModelError("", ex.Message);
            return View(vm);
        }

        return RedirectToAction(nameof(Tags));
    }

    // Открывает форму редактирования категории.
    [HttpGet]
    public async Task<IActionResult> EditTag(Guid id)
    {
        var tag = await _tagService.GetByIdAsync(id);

        if (tag == null)
            return NotFound();

        var vm = new TagEditVM
        {
            Id = tag.Id,
            Name = tag.Name
        };

        return View(vm);
    }

    // Сохраняет изменения категории.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> EditTag(TagEditVM vm)
    {
        if (!ModelState.IsValid)
            return View(vm);

        try
        {
            await _tagService.UpdateAsync(vm);
        }
        catch (Exception ex)
        {
            ModelState.AddModelError("", ex.Message);
            return View(vm);
        }

        return RedirectToAction(nameof(Tags));
    }

    // Удаляет категорию после подтверждения.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteTag(Guid id)
    {
        try
        {
            await _tagService.DeleteAsync(id);
        }
        catch (Exception ex)
        {
            TempData["TagError"] = ex.Message;
        }

        return RedirectToAction(nameof(Tags));
    }
}
