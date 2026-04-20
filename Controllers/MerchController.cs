using HsoPkipt.Services.Interfaces;
using HsoPkipt.ViewModels.Merch;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace HsoPkipt.Controllers;

// Контроллер отвечает за каталог мерча и корзину.
public class MerchController : Controller
{
    // Сервис каталога товаров.
    private readonly IMerchService _merchService;

    // Сервис корзины.
    private readonly IMerchCartService _merchCartService;

    // Получаем сервисы через конструктор.
    public MerchController(IMerchService merchService, IMerchCartService merchCartService)
    {
        _merchService = merchService;
        _merchCartService = merchCartService;
    }

    // Показывает весь каталог мерча.
    public async Task<IActionResult> Index()
    {
        var items = await _merchService.GetAllAsync();
        return View(await BuildCatalogAsync(items));
    }

    // Показывает товары только одной категории.
    public async Task<IActionResult> Category(Guid id)
    {
        var items = await _merchService.GetByCategoryAsync(id);
        return View("Index", await BuildCatalogAsync(items));
    }

    // Ищет товары по строке поиска.
    [HttpGet]
    public async Task<IActionResult> Search(string q)
    {
        var items = await _merchService.SearchAsync(q ?? string.Empty);
        return View("Index", await BuildCatalogAsync(items, q));
    }

    // Показывает корзину пользователя.
    [HttpGet]
    public async Task<IActionResult> Cart()
    {
        return View(await BuildCartPageAsync());
    }

    // Добавляет товар в корзину.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> AddToCart(Guid merchItemId, string? returnUrl)
    {
        await _merchCartService.AddAsync(merchItemId);
        return RedirectToLocal(returnUrl);
    }

    // Меняет количество товара в корзине.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> UpdateCartItem(Guid merchItemId, int quantity)
    {
        await _merchCartService.UpdateQuantityAsync(merchItemId, quantity);
        return RedirectToAction(nameof(Cart));
    }

    // Удаляет один товар из корзины.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> RemoveFromCart(Guid merchItemId)
    {
        await _merchCartService.RemoveAsync(merchItemId);
        return RedirectToAction(nameof(Cart));
    }

    // Полностью очищает корзину.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> ClearCart()
    {
        await _merchCartService.ClearAsync();
        return RedirectToAction(nameof(Cart));
    }

    // Оформляет заказ из текущей корзины.
    [Authorize]
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Checkout(MerchCartPageVM model)
    {
        // Если форма не прошла проверку, возвращаем страницу корзины с ошибками.
        if (!ModelState.IsValid)
        {
            return View("Cart", await BuildCartPageAsync(model.Checkout));
        }

        // Берём id текущего пользователя из его данных.
        var userIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);

        if (string.IsNullOrWhiteSpace(userIdClaim))
        {
            return Unauthorized();
        }

        var userId = Guid.Parse(userIdClaim);
        var result = await _merchCartService.CheckoutAsync(userId, model.Checkout);

        // Если корзина оказалась пустой, просто вернём пользователя назад.
        if (!result)
        {
            TempData["CartError"] = "Корзина пуста";
            return RedirectToAction(nameof(Cart));
        }

        TempData["CartSuccess"] = "Заказ успешно оформлен";
        return RedirectToAction(nameof(Cart));
    }

    // Собирает модель каталога вместе с корзиной и строкой поиска.
    private async Task<MerchCatalogVM> BuildCatalogAsync(List<MerchItemVM> items, string? searchQuery = null)
    {
        return new MerchCatalogVM
        {
            Items = items,
            Cart = await _merchCartService.GetCartAsync(),
            SearchQuery = searchQuery ?? string.Empty
        };
    }

    // Собирает модель для страницы корзины.
    private async Task<MerchCartPageVM> BuildCartPageAsync(CheckoutOrderVM? checkout = null)
    {
        return new MerchCartPageVM
        {
            Cart = await _merchCartService.GetCartAsync(),
            Checkout = checkout ?? new CheckoutOrderVM(),
            IsAuthenticated = User.Identity?.IsAuthenticated == true
        };
    }

    // Возвращает пользователя на локальный адрес, если он безопасный.
    private IActionResult RedirectToLocal(string? returnUrl)
    {
        if (!string.IsNullOrWhiteSpace(returnUrl) && Url.IsLocalUrl(returnUrl))
        {
            return LocalRedirect(returnUrl);
        }

        return RedirectToAction(nameof(Index));
    }
}
