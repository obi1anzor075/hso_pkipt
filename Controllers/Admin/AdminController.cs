using HsoPkipt.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HsoPkipt.Controllers.Admin;

[Authorize(Roles = Roles.Admin + "," + Roles.Moderator)]
public class AdminController : Controller
{
    public IActionResult Index()
    {
        return View();
    }

    [Authorize(Roles = Roles.Admin)]
    public IActionResult News()
    {
        return View();
    }

    [Authorize(Roles = Roles.Admin)]
    public IActionResult Users()
    {
        return View();
    }

    [Authorize(Roles = Roles.Admin)]
    public IActionResult Projects()
    {
        return View();
    }

    public IActionResult Events()
    {
        return View();
    }

    [Authorize(Roles = Roles.Admin)]
    public IActionResult Tags()
    {
        return View();
    }
}