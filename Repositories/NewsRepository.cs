using HsoPkipt.Models;
using HsoPkipt.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace HsoPkipt.Repositories;

public class NewsRepository : INewsRepository
{
    // контекст базы данных
    private readonly AppDbContext _context;

    // сохраняем контекст для дальнейшей работы с новостями
    public NewsRepository(AppDbContext context)
    {
        _context = context;
    }

    // считаем общее количество новостей
    public async Task<int> CountAsync()
    {
        return await _context.News.CountAsync();
    }

    // добавляем новую новость в базу
    public async Task CreateAsync(NewsItem item)
    {
        _context.News.Add(item);
        await _context.SaveChangesAsync();
    }

    // удаляем новость из базы
    public async Task DeleteAsync(NewsItem item)
    {
        _context.News.Remove(item);
        await _context.SaveChangesAsync();
    }

    // получаем новость по id вместе с тегами
    public async Task<NewsItem?> GetByIdAsync(Guid id)
    {
        return await _context.News
            .Include(n => n.Tags)
            .FirstOrDefaultAsync(n => n.Id == id);
    }

    // берём последние новости по дате создания
    public async Task<List<NewsItem>> GetLatestAsync(int count = 1)
    {
        return await _context.News
            .OrderByDescending(n => n.CreatedAt)
            .Take(count)
            .ToListAsync();
    }

    // берём часть новостей для пагинации или выборочной загрузки
    public async Task<List<NewsItem>> GetRangeAsync(int start, int take)
    {
        return await _context.News
            .OrderByDescending(n => n.CreatedAt)
            .Skip(start)
            .Take(take)
            .ToListAsync();
    }

    // получаем опубликованные новости для нужной страницы
    public async Task<(List<NewsItem> items, int count)> GetPagedAsync(int pageNumber, int pageSize)
    {
        var query = _context.News
            .AsNoTracking()
            .Where(n => n.IsPublished);

        // считаем общее количество записей для пагинации
        int totalCount = query.Count();

        // загружаем только новости текущей страницы
        var items = await query
            .Include(n => n.Tags)
            .OrderByDescending(n => n.CreatedAt)
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        return (items, totalCount);
    }

    // обновляем существующую новость
    public async Task UpdateAsync(NewsItem item)
    {
        _context.News.Update(item);
        await _context.SaveChangesAsync();
    }
}