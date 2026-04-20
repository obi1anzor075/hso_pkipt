using HsoPkipt.ViewModels.Merch;

namespace HsoPkipt.Services.Interfaces;

public interface IMerchCartService
{
    // Возвращает текущую корзину.
    Task<MerchCartVM> GetCartAsync();

    // Возвращает количество товаров в корзине.
    Task<int> GetItemsCountAsync();

    // Добавляет товар в корзину.
    Task AddAsync(Guid merchItemId, int quantity = 1);

    // Меняет количество у товара в корзине.
    Task UpdateQuantityAsync(Guid merchItemId, int quantity);

    // Убирает товар из корзины.
    Task RemoveAsync(Guid merchItemId);

    // Полностью очищает корзину.
    Task ClearAsync();

    // Создаёт заказ по данным из корзины.
    Task<bool> CheckoutAsync(Guid userId, CheckoutOrderVM model);
}
