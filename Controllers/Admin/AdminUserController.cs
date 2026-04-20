using HsoPkipt.Identity;
using HsoPkipt.Services.Interfaces;
using HsoPkipt.ViewModels.Users;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HsoPkipt.Controllers.Admin;

// Контроллер для управления пользователями в админке.
[Authorize(Roles = Roles.Admin)]
public class AdminUserController : Controller
{
    // Сервис списка и редактирования пользователей.
    private readonly IUserService _userService;

    // Сервис регистрации нужен для создания новых пользователей.
    private readonly IIdentityService _identityService;

    // Получаем зависимости через конструктор.
    public AdminUserController(
        IUserService userService,
        IIdentityService identityService)
    {
        _userService = userService;
        _identityService = identityService;
    }

    // Показывает страницу со списком пользователей.
    [HttpGet]
    public async Task<IActionResult> Index(int page = 1)
    {
        const int pageSize = 10;

        var result = await _userService.GetUsersPageAsync(page, pageSize);

        return View(result);
    }

    // Открывает форму создания пользователя.
    [HttpGet]
    public IActionResult Create()
    {
        return View(new CreateUserVM());
    }

    // Сохраняет нового пользователя.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(CreateUserVM model)
    {
        if (!ModelState.IsValid)
            return View(model);

        var result = await _identityService.CreateUserAsync(model);

        if (!result.Succeeded)
        {
            foreach (var error in result.Errors)
                ModelState.AddModelError(string.Empty, error.Description);

            return View(model);
        }

        return RedirectToAction(nameof(Index));
    }

    // Открывает форму редактирования пользователя.
    [HttpGet]
    public async Task<IActionResult> Edit(Guid id)
    {
        var user = await _userService.GetForUpdateAsync(id);

        if (user == null)
            return NotFound();

        return View(user);
    }

    // Сохраняет изменения пользователя.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(Guid id, UpdateUserVM model)
    {
        if (!ModelState.IsValid)
            return View(model);

        var updated = await _userService.UpdateAsync(id, model);

        if (!updated)
            return NotFound();

        return RedirectToAction(nameof(Index));
    }

    // Показывает страницу удаления пользователя.
    [HttpGet]
    public async Task<IActionResult> Delete(Guid id)
    {
        var user = await _userService.GetByIdAsync(id);

        if (user == null)
            return NotFound();

        return View(user);
    }

    // Удаляет пользователя после подтверждения.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(Guid id)
    {
        var deleted = await _userService.DeleteAsync(id);

        if (!deleted)
            return NotFound();

        return RedirectToAction(nameof(Index));
    }
}
