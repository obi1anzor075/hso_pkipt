using HsoPkipt.Extensions;
using HsoPkipt.Models;
using HsoPkipt.Repositories.Interfaces;
using HsoPkipt.Services.Interfaces;
using HsoPkipt.ViewModels.Merch;
using Microsoft.AspNetCore.Http;

namespace HsoPkipt.Services;

// Сервис хранит корзину в сессии и умеет превращать её в заказ.
public class MerchCartService : IMerchCartService
{
    // Ключ, под которым корзина лежит в сессии.
    private const string SessionKey = "merch-cart";

    // Доступ к текущему запросу и сессии.
    private readonly IHttpContextAccessor _httpContextAccessor;

    // Репозиторий товаров нужен, чтобы подтянуть актуальные данные по товару.
    private readonly IMerchRepository _merchRepository;

    // Репозиторий заказов нужен при оформлении покупки.
    private readonly IOrderRepository _orderRepository;

    // Получаем зависимости через конструктор.
    public MerchCartService(
        IHttpContextAccessor httpContextAccessor,
        IMerchRepository merchRepository,
        IOrderRepository orderRepository)
    {
        _httpContextAccessor = httpContextAccessor;
        _merchRepository = merchRepository;
        _orderRepository = orderRepository;
    }

    // Собирает полную корзину для страницы пользователя.
    public async Task<MerchCartVM> GetCartAsync()
    {
        var session = GetSession();
        var state = GetState(session);
        var items = new List<CartItemVM>();

        foreach (var entry in state)
        {
            var merchItem = await _merchRepository.GetByIdAsync(entry.MerchItemId);

            if (merchItem is null)
                continue;

            items.Add(new CartItemVM
            {
                Id = merchItem.Id,
                Name = merchItem.Name,
                ImageUrl = merchItem.ImageUrl,
                Price = merchItem.Price,
                Quantity = entry.Quantity
            });
        }

        // Если какого-то товара больше нет, сразу чистим корзину от старой записи.
        if (items.Count != state.Count)
        {
            SaveState(session, items.Select(x => new CartStateItem(x.Id, x.Quantity)).ToList());
        }

        return new MerchCartVM
        {
            Items = items,
            ItemsCount = items.Sum(x => x.Quantity),
            TotalPrice = items.Sum(x => x.TotalPrice)
        };
    }

    // Возвращает только количество товаров без полной модели.
    public async Task<int> GetItemsCountAsync()
    {
        var cart = await GetCartAsync();
        return cart.ItemsCount;
    }

    // Добавляет товар в корзину.
    public async Task AddAsync(Guid merchItemId, int quantity = 1)
    {
        if (quantity <= 0)
            return;

        var merchItem = await _merchRepository.GetByIdAsync(merchItemId);

        if (merchItem is null)
            return;

        var session = GetSession();
        var state = GetState(session);
        var existing = state.FirstOrDefault(x => x.MerchItemId == merchItemId);

        if (existing is null)
        {
            state.Add(new CartStateItem(merchItemId, quantity));
        }
        else
        {
            existing.Quantity += quantity;
        }

        SaveState(session, state);
    }

    // Меняет количество товара.
    public Task UpdateQuantityAsync(Guid merchItemId, int quantity)
    {
        var session = GetSession();
        var state = GetState(session);
        var existing = state.FirstOrDefault(x => x.MerchItemId == merchItemId);

        if (existing is null)
            return Task.CompletedTask;

        if (quantity <= 0)
        {
            state.Remove(existing);
        }
        else
        {
            existing.Quantity = quantity;
        }

        SaveState(session, state);
        return Task.CompletedTask;
    }

    // Полностью убирает один товар из корзины.
    public Task RemoveAsync(Guid merchItemId)
    {
        var session = GetSession();
        var state = GetState(session);
        state.RemoveAll(x => x.MerchItemId == merchItemId);
        SaveState(session, state);

        return Task.CompletedTask;
    }

    // Очищает всю корзину целиком.
    public Task ClearAsync()
    {
        GetSession().Remove(SessionKey);
        return Task.CompletedTask;
    }

    // Создаёт заказ из содержимого корзины.
    public async Task<bool> CheckoutAsync(Guid userId, CheckoutOrderVM model)
    {
        var cart = await GetCartAsync();

        if (cart.IsEmpty)
            return false;

        var items = cart.Items
            .Select(x => new OrderItem(x.Id, x.Name, x.Price, x.Quantity))
            .ToList();

        var order = new Order(
            userId,
            model.RecipientName,
            model.PhoneNumber,
            model.DeliveryAddress,
            items);

        await _orderRepository.CreateAsync(order);
        await ClearAsync();

        return true;
    }

    // Возвращает текущую сессию запроса.
    private ISession GetSession()
    {
        var session = _httpContextAccessor.HttpContext?.Session;

        if (session is null)
            throw new InvalidOperationException("Session is not available for the current request.");

        return session;
    }

    // Читает корзину из сессии.
    private static List<CartStateItem> GetState(ISession session)
    {
        return session.GetJson<List<CartStateItem>>(SessionKey) ?? [];
    }

    // Сохраняет корзину обратно в сессию.
    private static void SaveState(ISession session, List<CartStateItem> state)
    {
        session.SetJson(SessionKey, state);
    }

    // Это внутренняя модель, в которой мы держим корзину в сессии.
    private sealed class CartStateItem
    {
        // Id товара.
        public Guid MerchItemId { get; set; }

        // Сколько штук положили в корзину.
        public int Quantity { get; set; }

        // Нужен, когда создаём запись вручную.
        public CartStateItem(Guid merchItemId, int quantity)
        {
            MerchItemId = merchItemId;
            Quantity = quantity;
        }

        // Нужен, когда объект читается из JSON.
        public CartStateItem()
        {
        }
    }
}
