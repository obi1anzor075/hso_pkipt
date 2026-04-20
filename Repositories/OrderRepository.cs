using HsoPkipt.Models;
using HsoPkipt.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace HsoPkipt.Repositories;

// Репозиторий сохраняет заказы и отдаёт историю заказов.
public class OrderRepository : IOrderRepository
{
    // Контекст базы данных.
    private readonly AppDbContext _context;

    // Получаем контекст через конструктор.
    public OrderRepository(AppDbContext context)
    {
        _context = context;
    }

    // Создаёт новый заказ.
    public async Task CreateAsync(Order order)
    {
        await _context.Orders.AddAsync(order);
        await _context.SaveChangesAsync();
    }

    // Возвращает все заказы одного пользователя.
    public async Task<List<Order>> GetByUserIdAsync(Guid userId)
    {
        return await _context.Orders
            .Include(x => x.Items)
            .Where(x => x.UserId == userId)
            .OrderByDescending(x => x.CreatedAt)
            .ToListAsync();
    }
}
