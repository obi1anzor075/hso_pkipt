using HsoPkipt.Identity;
using HsoPkipt.Services.Interfaces;
using HsoPkipt.ViewModels.Tags;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HsoPkipt.Controllers.Admin;

[Authorize(Roles = Roles.Admin)]
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

    [HttpGet]
    public IActionResult CreateTag()
    {
        return View(new TagCreateVM());
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
