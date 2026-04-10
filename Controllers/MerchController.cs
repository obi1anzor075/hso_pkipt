using HsoPkipt.Services.Interfaces;
using HsoPkipt.ViewModels.Merch;
using Microsoft.AspNetCore.Mvc;

namespace HsoPkipt.Controllers;

public class MerchController : Controller
{
    private readonly IMerchService _merchService;
    private readonly IMerchCartService _merchCartService;

    public MerchController(IMerchService merchService, IMerchCartService merchCartService)
    {
        _merchService = merchService;
        _merchCartService = merchCartService;
    }

    public async Task<IActionResult> Index()
    {
        var items = await _merchService.GetAllAsync();
        return View(await BuildCatalogAsync(items));
    }

    public async Task<IActionResult> Category(Guid id)
    {
        var items = await _merchService.GetByCategoryAsync(id);
        return View("Index", await BuildCatalogAsync(items));
    }

    [HttpGet]
    public async Task<IActionResult> Search(string q)
    {
        var items = await _merchService.SearchAsync(q ?? string.Empty);
        return View("Index", await BuildCatalogAsync(items, q));
    }

    [HttpGet]
    public async Task<IActionResult> Cart()
    {
        var cart = await _merchCartService.GetCartAsync();
        return View(cart);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> AddToCart(Guid merchItemId, string? returnUrl)
    {
        await _merchCartService.AddAsync(merchItemId);
        return RedirectToLocal(returnUrl);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> UpdateCartItem(Guid merchItemId, int quantity)
    {
        await _merchCartService.UpdateQuantityAsync(merchItemId, quantity);
        return RedirectToAction(nameof(Cart));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> RemoveFromCart(Guid merchItemId)
    {
        await _merchCartService.RemoveAsync(merchItemId);
        return RedirectToAction(nameof(Cart));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> ClearCart()
    {
        await _merchCartService.ClearAsync();
        return RedirectToAction(nameof(Cart));
    }

    private async Task<MerchCatalogVM> BuildCatalogAsync(List<MerchItemVM> items, string? searchQuery = null)
    {
        return new MerchCatalogVM
        {
            Items = items,
            Cart = await _merchCartService.GetCartAsync(),
            SearchQuery = searchQuery ?? string.Empty
        };
    }

    private IActionResult RedirectToLocal(string? returnUrl)
    {
        if (!string.IsNullOrWhiteSpace(returnUrl) && Url.IsLocalUrl(returnUrl))
        {
            return LocalRedirect(returnUrl);
        }

        return RedirectToAction(nameof(Index));
    }
}
