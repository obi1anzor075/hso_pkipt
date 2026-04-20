using HsoPkipt.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HsoPkipt.Controllers.Admin;

// Вход в разные разделы админки.
[Authorize(Roles = Roles.Admin + "," + Roles.Moderator)]
public class AdminController : Controller
{
    // Главная страница админки.
    public IActionResult Index()
    {
        return View();
    }

    // Раздел управления новостями.
    [Authorize(Roles = Roles.Admin)]
    public IActionResult News()
    {
        return View();
    }

    // Раздел управления пользователями.
    [Authorize(Roles = Roles.Admin)]
    public IActionResult Users()
    {
        return View();
    }

    // Раздел управления проектами.
    [Authorize(Roles = Roles.Admin)]
    public IActionResult Projects()
    {
        return View();
    }

    // Раздел управления мерчем.
    [Authorize(Roles = Roles.Admin)]
    public IActionResult Merch()
    {
        return View();
    }

    // Раздел управления событиями.
    public IActionResult Events()
    {
        return View();
    }

    // Раздел управления категориями.
    [Authorize(Roles = Roles.Admin)]
    public IActionResult Tags()
    {
        return View();
    }
}
