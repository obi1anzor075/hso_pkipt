using HsoPkipt.Identity;
using HsoPkipt.Services.Interfaces;
using HsoPkipt.ViewModels.Users;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HsoPkipt.Controllers.Admin;

[Authorize(Roles = Roles.Admin)]
public class AdminUserController : Controller
{
    private readonly IUserService _userService;
    private readonly IIdentityService _identityService;

    public AdminUserController(
        IUserService userService,
        IIdentityService identityService)
    {
        _userService = userService;
        _identityService = identityService;
    }

    [HttpGet]
    public async Task<IActionResult> Index(int page = 1)
    {
        const int pageSize = 10;

        var result = await _userService.GetUsersPageAsync(page, pageSize);

        return View(result);
    }

    [HttpGet]
    public IActionResult Create()
    {
        return View(new CreateUserVM());
    }

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

    [HttpGet]
    public async Task<IActionResult> Edit(Guid id)
    {
        var user = await _userService.GetForUpdateAsync(id);

        if (user == null)
            return NotFound();

        return View(user);
    }

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

    [HttpGet]
    public async Task<IActionResult> Delete(Guid id)
    {
        var user = await _userService.GetByIdAsync(id);

        if (user == null)
            return NotFound();

        return View(user);
    }

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