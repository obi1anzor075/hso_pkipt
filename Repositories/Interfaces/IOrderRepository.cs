using HsoPkipt.Models;

namespace HsoPkipt.Repositories.Interfaces;

public interface IOrderRepository
{
    // Сохраняет новый заказ.
    Task CreateAsync(Order order);

    // Возвращает заказы конкретного пользователя.
    Task<List<Order>> GetByUserIdAsync(Guid userId);
}
