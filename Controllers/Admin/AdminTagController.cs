using HsoPkipt.Services.Interfaces;
using HsoPkipt.ViewModels.Tags;
using Microsoft.AspNetCore.Mvc;

namespace HsoPkipt.Controllers.Admin;

public class AdminTagController : Controller
{
    private readonly ITagService _tagService;

    public AdminTagController(ITagService tagService)
    {
        _tagService = tagService;
    }

    public async Task<IActionResult> Tags()
    {
        var tags = await _tagService.GetAllAsync();
        return View(tags);
    }

    // Создание
    [HttpGet]
    public IActionResult CreateTag()
    {
        return View();
    }

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

    // Редактирование
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

    // Удаление
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteTag(Guid id)
    {
        await _tagService.DeleteAsync(id);
        return RedirectToAction(nameof(Tags));
    }
}