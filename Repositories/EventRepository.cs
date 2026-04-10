using HsoPkipt.Models;
using HsoPkipt.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace HsoPkipt.Repositories;

public class EventRepository : IEventRepository
{
    private readonly AppDbContext _context;

    public EventRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<List<Event>> GetAllAsync()
    {
        var now = DateTime.UtcNow;

        return await _context.Events
            .Where(e => e.EventDate >= now)
            .OrderBy(e => e.EventDate)
            .ToListAsync();
    }

    public async Task<Event?> GetByIdAsync(Guid id)
    {
        return await _context.Events
            .FirstOrDefaultAsync(e => e.Id == id);
    }

    public async Task AddAsync(Event entity)
    {
        await _context.Events.AddAsync(entity);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(Event entity)
    {
        _context.Events.Update(entity);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(Event entity)
    {
        _context.Events.Remove(entity);
        await _context.SaveChangesAsync();
    }

    public async Task<(List<Event> items, int count)> GetPagedAsync(int pageNumber, int pageSize)
    {
        var query = _context.Events
            .AsNoTracking();

        int totalCount = query.Count();

        var items = await query
            .OrderByDescending(n => n.Id)
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        return (items, totalCount);
    }
}