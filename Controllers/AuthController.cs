using HsoPkipt.Services.Interfaces;
using HsoPkipt.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace HsoPkipt.Controllers;

public class AuthController : Controller
{
    private readonly IIdentityService _identityService;

    public AuthController(IIdentityService identityService)
    {
        _identityService = identityService;
    }

    public async Task<IActionResult> Login()
    {
        if (User.Identity.IsAuthenticated)
        {
            return RedirectToAction("Index", "Home");
        }

        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Login(LoginVM model)
    {
        if (!ModelState.IsValid)
        {
            return View(model);
        }

        var result = await _identityService.SignInAsync(model.Email, model.Password, model.RememberMe);

        if (!result.Succeeded)
        {
            ModelState.AddModelError(string.Empty, "Неверные данные");
            return View(model);
        }

        return RedirectToAction("Index", "Home");
    }

    [HttpPost]
    public async Task<IActionResult> Logout()
    {
        await _identityService.SignOutAsync();
        return RedirectToAction("Index", "Home");
    }
}
