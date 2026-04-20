using HsoPkipt.Services.Interfaces;
using HsoPkipt.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace HsoPkipt.Controllers;

// Контроллер отвечает за вход и выход пользователя.
public class AuthController : Controller
{
    // Через сервис работаем с авторизацией.
    private readonly IIdentityService _identityService;

    // Получаем сервис через конструктор.
    public AuthController(IIdentityService identityService)
    {
        _identityService = identityService;
    }

    // Показывает страницу входа.
    public async Task<IActionResult> Login()
    {
        // Если пользователь уже вошёл, нет смысла снова открывать форму.
        if (User.Identity.IsAuthenticated)
        {
            return RedirectToAction("Index", "Home");
        }

        return View();
    }

    // Принимает данные формы входа.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Login(LoginVM model)
    {
        // Если форма заполнена неверно, просто показываем её обратно.
        if (!ModelState.IsValid)
        {
            return View(model);
        }

        // Пробуем авторизовать пользователя.
        var result = await _identityService.SignInAsync(model.Email, model.Password, model.RememberMe);

        // Если логин или пароль не подошли, показываем ошибку.
        if (!result.Succeeded)
        {
            ModelState.AddModelError(string.Empty, "Неверные данные");
            return View(model);
        }

        return RedirectToAction("Index", "Home");
    }

    // Завершает сессию пользователя.
    [HttpPost]
    public async Task<IActionResult> Logout()
    {
        await _identityService.SignOutAsync();
        return RedirectToAction("Index", "Home");
    }
}
