using HsoPkipt.Identity;
using HsoPkipt.Services;
using HsoPkipt.Services.Interfaces;
using HsoPkipt.ViewModels.Events;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HsoPkipt.Controllers.Admin;

[Authorize(Roles = Roles.Admin + "," + Roles.Moderator)]
public class AdminEventController : Controller
{
    private readonly IEventService _eventService;

    public AdminEventController(IEventService eventService)
    {
        _eventService = eventService;
    }

    [HttpGet]
    public async Task<IActionResult> Index(int page = 1)
    {
        const int pageSize = 10;

        var result = await _eventService.GetPageAsync(page, pageSize);

        return View(result);
    }

    // Создание
    [HttpGet]
    public IActionResult Create()
    {
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(EventCreateVM vm)
    {
        if (!ModelState.IsValid)
            return View(vm);

        await _eventService.CreateAsync(vm);

        return RedirectToAction(nameof(Index));
    }

    // Редактирование
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

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(EventEditVM vm)
    {
        if (!ModelState.IsValid)
            return View(vm);

        await _eventService.UpdateAsync(vm);

        return RedirectToAction(nameof(Index));
    }

    // удаление
    [HttpGet]
    public async Task<IActionResult> Delete(Guid id)
    {
        var eventVm= await _eventService.GetByIdAsync(id);

        if (eventVm == null)
            return NotFound();

        return View(eventVm);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(Guid id)
    {
        await _eventService.DeleteAsync(id);

        return RedirectToAction(nameof(Index));
    }
}