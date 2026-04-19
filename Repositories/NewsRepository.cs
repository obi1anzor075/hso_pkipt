using HsoPkipt.Models;
using HsoPkipt.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace HsoPkipt.Repositories;

// Репозиторий работает с новостями напрямую через базу.
public class NewsRepository : INewsRepository
{
    // Контекст нужен, чтобы читать и сохранять данные.
    private readonly AppDbContext _context;

    // Получаем контекст через конструктор.
    public NewsRepository(AppDbContext context)
    {
        _context = context;
    }

    // Возвращает общее число новостей.
    public async Task<int> CountAsync()
    {
        return await _context.News.CountAsync();
    }

    // Сохраняет новую новость.
    public async Task CreateAsync(NewsItem item)
    {
        _context.News.Add(item);
        await _context.SaveChangesAsync();
    }

    // Удаляет новость.
    public async Task DeleteAsync(NewsItem item)
    {
        _context.News.Remove(item);
        await _context.SaveChangesAsync();
    }

    // Ищет новость по id.
    public async Task<NewsItem?> GetByIdAsync(Guid id)
    {
        return await _context.News.FirstOrDefaultAsync(n => n.Id == id);
    }

    // Берёт несколько последних новостей.
    public async Task<List<NewsItem>> GetLatestAsync(int count = 1)
    {
        return await _context.News
            .OrderByDescending(n => n.CreatedAt)
            .Take(count)
            .ToListAsync();
    }

    // Возвращает кусок списка новостей.
    public async Task<List<NewsItem>> GetRangeAsync(int start, int take)
    {
        return await _context.News
            .OrderByDescending(n => n.CreatedAt)
            .Skip(start)
            .Take(take)
            .ToListAsync();
    }

    // Возвращает страницу новостей и общее число записей.
    public async Task<(List<NewsItem> items, int count)> GetPagedAsync(int pageNumber, int pageSize)
    {
        var query = _context.News.AsNoTracking();

        int totalCount = query.Count();

        var items = await query
            .OrderByDescending(n => n.CreatedAt)
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        return (items, totalCount);
    }

    // Сохраняет изменения в уже существующей новости.
    public async Task UpdateAsync(NewsItem item)
    {
        _context.News.Update(item);
        await _context.SaveChangesAsync();
    }
}
