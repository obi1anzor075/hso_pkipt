using HsoPkipt.ViewModels.Merch;

namespace HsoPkipt.Services.Interfaces;

public interface IMerchCartService
{
    Task<MerchCartVM> GetCartAsync();
    Task<int> GetItemsCountAsync();
    Task AddAsync(Guid merchItemId, int quantity = 1);
    Task UpdateQuantityAsync(Guid merchItemId, int quantity);
    Task RemoveAsync(Guid merchItemId);
    Task ClearAsync();
    Task<bool> CheckoutAsync(Guid userId, CheckoutOrderVM model);
}
