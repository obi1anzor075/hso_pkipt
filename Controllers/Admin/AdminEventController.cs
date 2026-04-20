using HsoPkipt.Identity;
using HsoPkipt.Services.Interfaces;
using HsoPkipt.ViewModels.Events;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HsoPkipt.Controllers.Admin;

// Этот контроллер нужен для работы с событиями в админке.
[Authorize(Roles = Roles.Admin + "," + Roles.Moderator)]
public class AdminEventController : Controller
{
    // Сервис событий.
    private readonly IEventService _eventService;

    // Получаем сервис через конструктор.
    public AdminEventController(IEventService eventService)
    {
        _eventService = eventService;
    }

    // Показывает страницу со списком событий.
    [HttpGet]
    public async Task<IActionResult> Index(int page = 1)
    {
        const int pageSize = 10;

        var result = await _eventService.GetPageAsync(page, pageSize);

        return View(result);
    }

    // Открывает форму создания события.
    [HttpGet]
    public IActionResult Create()
    {
        return View();
    }

    // Сохраняет новое событие.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(EventCreateVM vm)
    {
        if (!ModelState.IsValid)
            return View(vm);

        await _eventService.CreateAsync(vm);

        return RedirectToAction(nameof(Index));
    }

    // Открывает форму редактирования.
    [HttpGet]
    public async Task<IActionResult> Edit(Guid id)
    {
        var eventVm = await _eventService.GetByIdAsync(id);

        if (eventVm == null)
            return NotFound();

        var editVm = new EventEditVM
        {
            Id = eventVm.Id,
            Title = eventVm.Title,
            EventDate = eventVm.EventDate,
            Description = eventVm.Description,
            IsPublished = eventVm.IsPublished
        };

        return View(editVm);
    }

    // Сохраняет изменения события.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(EventEditVM vm)
    {
        if (!ModelState.IsValid)
            return View(vm);

        await _eventService.UpdateAsync(vm);

        return RedirectToAction(nameof(Index));
    }

    // Показывает страницу подтверждения удаления.
    [HttpGet]
    public async Task<IActionResult> Delete(Guid id)
    {
        var eventVm = await _eventService.GetByIdAsync(id);

        if (eventVm == null)
            return NotFound();

        return View(eventVm);
    }

    // Удаляет событие после подтверждения.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(Guid id)
    {
        await _eventService.DeleteAsync(id);

        return RedirectToAction(nameof(Index));
    }
}
