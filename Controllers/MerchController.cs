using HsoPkipt.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace HsoPkipt.Controllers;

public class MerchController : Controller
{
    private readonly IMerchService _merchService;

    public MerchController(IMerchService merchService)
    {
        _merchService = merchService;
    }

    public async Task<IActionResult> Index()
    {
        var items = await _merchService.GetAllAsync();
        return View(items);
    }

    public async Task<IActionResult> Category(Guid id)
    {
        var items = await _merchService.GetByCategoryAsync(id);

        return View(items);
    }

    [HttpGet]
    public async Task<IActionResult> Search(string q)
    {
        var items = await _merchService.SearchAsync(q ?? "");
        return View("Index", items);
    }
}