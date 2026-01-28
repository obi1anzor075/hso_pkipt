using HsoPkipt.Data;
using HsoPkipt.Models;
using HsoPkipt.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace HsoPkipt.Repositories;

public class NewsRepository : INewsRepository
{
    private readonly AppDbContext _context;

    public NewsRepository(AppDbContext context)
    {
        _context = context;
    }
    public async Task<int> CountAsync()
    {
        return await _context.News.CountAsync();
    }

    public async Task CreateAsync(NewsItem item)
    {
        _context.News.Add(item);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(NewsItem item)
    {
        _context.News.Remove(item);
        await _context.SaveChangesAsync();
    }

    public async Task<NewsItem> GetByIdAsync(Guid id)
    {
        return await _context.News.FirstOrDefaultAsync(n => n.Id == id);
    }

    public async Task<List<NewsItem>> GetLatestAsync(int count = 1)
    {
        return await _context.News
            .OrderByDescending(n => n.CreatedAt)
            .Take(count)
            .ToListAsync();
    }

    public async Task<List<NewsItem>> GetRangeAsync(int start, int take)
    {
        return await _context.News
            .OrderByDescending(n => n.CreatedAt)
            .Skip(start)
            .Take(take)
            .ToListAsync();
    }

    public async Task UpdateAsync(NewsItem item)
    {
        _context.News.Update(item);
        await _context.SaveChangesAsync();
    }
}
