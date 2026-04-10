using HsoPkipt.Extensions;
using HsoPkipt.Repositories.Interfaces;
using HsoPkipt.Services.Interfaces;
using HsoPkipt.ViewModels.Merch;
using Microsoft.AspNetCore.Http;

namespace HsoPkipt.Services;

public class MerchCartService : IMerchCartService
{
    private const string SessionKey = "merch-cart";

    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IMerchRepository _merchRepository;

    public MerchCartService(IHttpContextAccessor httpContextAccessor, IMerchRepository merchRepository)
    {
        _httpContextAccessor = httpContextAccessor;
        _merchRepository = merchRepository;
    }

    public async Task<MerchCartVM> GetCartAsync()
    {
        var session = GetSession();
        var state = GetState(session);
        var items = new List<CartItemVM>();

        foreach (var entry in state)
        {
            var merchItem = await _merchRepository.GetByIdAsync(entry.MerchItemId);
            if (merchItem is null)
            {
                continue;
            }

            items.Add(new CartItemVM
            {
                Id = merchItem.Id,
                Name = merchItem.Name,
                ImageUrl = merchItem.ImageUrl,
                Price = merchItem.Price,
                Quantity = entry.Quantity
            });
        }

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

    public async Task<int> GetItemsCountAsync()
    {
        var cart = await GetCartAsync();
        return cart.ItemsCount;
    }

    public async Task AddAsync(Guid merchItemId, int quantity = 1)
    {
        if (quantity <= 0)
        {
            return;
        }

        var merchItem = await _merchRepository.GetByIdAsync(merchItemId);
        if (merchItem is null)
        {
            return;
        }

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

    public Task UpdateQuantityAsync(Guid merchItemId, int quantity)
    {
        var session = GetSession();
        var state = GetState(session);
        var existing = state.FirstOrDefault(x => x.MerchItemId == merchItemId);

        if (existing is null)
        {
            return Task.CompletedTask;
        }

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

    public Task RemoveAsync(Guid merchItemId)
    {
        var session = GetSession();
        var state = GetState(session);
        state.RemoveAll(x => x.MerchItemId == merchItemId);
        SaveState(session, state);

        return Task.CompletedTask;
    }

    public Task ClearAsync()
    {
        GetSession().Remove(SessionKey);
        return Task.CompletedTask;
    }

    private ISession GetSession()
    {
        var session = _httpContextAccessor.HttpContext?.Session;

        if (session is null)
        {
            throw new InvalidOperationException("Session is not available for the current request.");
        }

        return session;
    }

    private static List<CartStateItem> GetState(ISession session)
    {
        return session.GetJson<List<CartStateItem>>(SessionKey) ?? [];
    }

    private static void SaveState(ISession session, List<CartStateItem> state)
    {
        session.SetJson(SessionKey, state);
    }

    private sealed class CartStateItem
    {
        public Guid MerchItemId { get; set; }
        public int Quantity { get; set; }

        public CartStateItem(Guid merchItemId, int quantity)
        {
            MerchItemId = merchItemId;
            Quantity = quantity;
        }

        public CartStateItem()
        {
        }
    }
}
