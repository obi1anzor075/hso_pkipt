using HsoPkipt.Services.Interfaces;
using HsoPkipt.ViewModels.Merch;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

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
        return View(await BuildCartPageAsync());
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

    [Authorize]
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Checkout(MerchCartPageVM model)
    {
        if (!ModelState.IsValid)
        {
            return View("Cart", await BuildCartPageAsync(model.Checkout));
        }

        var userIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);

        if (string.IsNullOrWhiteSpace(userIdClaim))
        {
            return Unauthorized();
        }

        var userId = Guid.Parse(userIdClaim);
        var result = await _merchCartService.CheckoutAsync(userId, model.Checkout);

        if (!result)
        {
            TempData["CartError"] = "Корзина пуста";
            return RedirectToAction(nameof(Cart));
        }

        TempData["CartSuccess"] = "Заказ успешно оформлен";
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

    private async Task<MerchCartPageVM> BuildCartPageAsync(CheckoutOrderVM? checkout = null)
    {
        return new MerchCartPageVM
        {
            Cart = await _merchCartService.GetCartAsync(),
            Checkout = checkout ?? new CheckoutOrderVM(),
            IsAuthenticated = User.Identity?.IsAuthenticated == true
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
