using HsoPkipt.Identity;
using HsoPkipt.Services.Interfaces;
using HsoPkipt.ViewModels.Merch;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace HsoPkipt.Controllers.Admin;

// Контроллер управляет товарами мерча в админке.
[Authorize(Roles = Roles.Admin)]
public class AdminMerchController : Controller
{
    // Сервис товаров.
    private readonly IMerchService _merchService;

    // Сервис категорий нужен для выпадающего списка.
    private readonly ITagService _tagService;

    // Получаем сервисы через конструктор.
    public AdminMerchController(IMerchService merchService, ITagService tagService)
    {
        _merchService = merchService;
        _tagService = tagService;
    }

    // Показывает список товаров.
    [HttpGet]
    public async Task<IActionResult> Index(int page = 1)
    {
        const int pageSize = 10;

        var result = await _merchService.GetPageAsync(page, pageSize);

        return View(result);
    }

    // Открывает форму создания товара.
    [HttpGet]
    public async Task<IActionResult> Create()
    {
        await LoadTagsAsync();
        return View(new CreateMerchItemVM());
    }

    // Сохраняет новый товар.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(CreateMerchItemVM model)
    {
        if (!ModelState.IsValid)
        {
            await LoadTagsAsync(model.TagId);
            return View(model);
        }

        await _merchService.CreateAsync(model);

        return RedirectToAction(nameof(Index));
    }

    // Открывает форму редактирования товара.
    [HttpGet]
    public async Task<IActionResult> Edit(Guid id)
    {
        var model = await _merchService.GetForUpdateAsync(id);

        if (model == null)
            return NotFound();

        await LoadTagsAsync(model.TagId);

        return View(model);
    }

    // Сохраняет изменения товара.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(Guid id, UpdateMerchItemVM model)
    {
        if (!ModelState.IsValid)
        {
            await LoadTagsAsync(model.TagId);
            return View(model);
        }

        var updated = await _merchService.UpdateAsync(id, model);

        if (!updated)
            return NotFound();

        return RedirectToAction(nameof(Index));
    }

    // Показывает страницу удаления товара.
    [HttpGet]
    public async Task<IActionResult> Delete(Guid id)
    {
        var item = await _merchService.GetByIdAsync(id);

        if (item == null)
            return NotFound();

        return View(item);
    }

    // Удаляет товар после подтверждения.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(Guid id)
    {
        var deleted = await _merchService.DeleteAsync(id);

        if (!deleted)
            return NotFound();

        return RedirectToAction(nameof(Index));
    }

    // Готовит список категорий для формы.
    private async Task LoadTagsAsync(Guid? selectedTagId = null)
    {
        var tags = await _tagService.GetAllAsync();

        ViewBag.Tags = tags.Select(x => new SelectListItem
        {
            Value = x.Id.ToString(),
            Text = x.Name,
            Selected = selectedTagId.HasValue && x.Id == selectedTagId.Value
        }).ToList();
    }
}
