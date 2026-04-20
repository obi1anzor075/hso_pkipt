using HsoPkipt.Models;
using HsoPkipt.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace HsoPkipt.Repositories;

// Репозиторий делает прямые запросы к таблице событий.
public class EventRepository : IEventRepository
{
    // Контекст базы данных.
    private readonly AppDbContext _context;

    // Получаем контекст через конструктор.
    public EventRepository(AppDbContext context)
    {
        _context = context;
    }

    // Возвращает все будущие события.
    public async Task<List<Event>> GetAllAsync()
    {
        var now = DateTime.UtcNow;

        return await _context.Events
            .Where(e => e.EventDate >= now)
            .OrderBy(e => e.EventDate)
            .ToListAsync();
    }

    // Ищет событие по id.
    public async Task<Event?> GetByIdAsync(Guid id)
    {
        return await _context.Events.FirstOrDefaultAsync(e => e.Id == id);
    }

    // Добавляет новое событие.
    public async Task AddAsync(Event entity)
    {
        await _context.Events.AddAsync(entity);
        await _context.SaveChangesAsync();
    }

    // Сохраняет изменения события.
    public async Task UpdateAsync(Event entity)
    {
        _context.Events.Update(entity);
        await _context.SaveChangesAsync();
    }

    // Удаляет событие.
    public async Task DeleteAsync(Event entity)
    {
        _context.Events.Remove(entity);
        await _context.SaveChangesAsync();
    }

    // Возвращает одну страницу событий и общее число записей.
    public async Task<(List<Event> items, int count)> GetPagedAsync(int pageNumber, int pageSize)
    {
        var query = _context.Events.AsNoTracking();
        int totalCount = query.Count();

        var items = await query
            .OrderByDescending(n => n.Id)
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        return (items, totalCount);
    }
}
