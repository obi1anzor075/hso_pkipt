using HsoPkipt.Models;
using HsoPkipt.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace HsoPkipt.Repositories;

// Репозиторий работает с товарами мерча.
public class MerchRepository : IMerchRepository
{
    // Контекст базы данных.
    private readonly AppDbContext _context;

    // Получаем контекст через конструктор.
    public MerchRepository(AppDbContext context)
    {
        _context = context;
    }

    // Возвращает все товары вместе с их категорией.
    public async Task<List<MerchItem>> GetAllAsync()
    {
        return await _context.MerchItems
            .Include(x => x.Tag)
            .ToListAsync();
    }

    // Возвращает товары только одной категории.
    public async Task<List<MerchItem>> GetByTagIdAsync(Guid tagId)
    {
        return await _context.MerchItems
            .Include(x => x.Tag)
            .Where(x => x.TagId == tagId)
            .ToListAsync();
    }

    // Ищет один товар по id.
    public async Task<MerchItem?> GetByIdAsync(Guid id)
    {
        return await _context.MerchItems
            .Include(x => x.Tag)
            .FirstOrDefaultAsync(x => x.Id == id);
    }

    // Добавляет новый товар.
    public async Task CreateAsync(MerchItem item)
    {
        await _context.MerchItems.AddAsync(item);
        await _context.SaveChangesAsync();
    }

    // Сохраняет изменения товара.
    public async Task UpdateAsync(MerchItem item)
    {
        _context.MerchItems.Update(item);
        await _context.SaveChangesAsync();
    }

    // Удаляет товар.
    public async Task DeleteAsync(MerchItem item)
    {
        _context.MerchItems.Remove(item);
        await _context.SaveChangesAsync();
    }

    // Ищет товары по названию и описанию.
    public async Task<List<MerchItem>> SearchAsync(string query)
    {
        return await _context.MerchItems
            .Include(x => x.Tag)
            .Where(x =>
                x.Name.Contains(query) ||
                (x.Description != null && x.Description.Contains(query)))
            .ToListAsync();
    }
}
