using HsoPkipt.Models;

namespace HsoPkipt.Repositories.Interfaces;

public interface IOrderRepository
{
    Task CreateAsync(Order order);
    Task<List<Order>> GetByUserIdAsync(Guid userId);
}
