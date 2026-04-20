using HsoPkipt.Models;
using HsoPkipt.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace HsoPkipt.Repositories;

// Репозиторий делает запросы к таблице проектов.
public class ProjectRepository : IProjectRepository
{
    // Контекст базы данных.
    private readonly AppDbContext _context;

    // Получаем контекст через конструктор.
    public ProjectRepository(AppDbContext context)
    {
        _context = context;
    }

    // Возвращает число всех проектов.
    public async Task<int> CountAsync()
    {
        return await _context.Projects.CountAsync();
    }

    // Добавляет новый проект.
    public async Task CreateAsync(ProjectItem item)
    {
        _context.Projects.Add(item);
        await _context.SaveChangesAsync();
    }

    // Удаляет проект.
    public async Task DeleteAsync(ProjectItem item)
    {
        _context.Projects.Remove(item);
        await _context.SaveChangesAsync();
    }

    // Ищет проект по id.
    public async Task<ProjectItem?> GetByIdAsync(Guid id)
    {
        return await _context.Projects.FirstOrDefaultAsync(p => p.Id == id);
    }

    // Возвращает последние проекты.
    public async Task<List<ProjectItem>> GetLatestAsync(int count = 1)
    {
        return await _context.Projects
            .OrderByDescending(p => p.CreatedAt)
            .Take(count)
            .ToListAsync();
    }

    // Возвращает одну страницу проектов и общее число записей.
    public async Task<(List<ProjectItem> items, int count)> GetPagedAsync(int pageNumber, int pageSize)
    {
        var query = _context.Projects.AsNoTracking();
        int totalCount = query.Count();

        var items = await query
            .OrderByDescending(p => p.CreatedAt)
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        return (items, totalCount);
    }

    // Возвращает часть списка проектов.
    public async Task<List<ProjectItem>> GetRangeAsync(int start, int take)
    {
        return await _context.Projects
            .OrderByDescending(p => p.CreatedAt)
            .Skip(start)
            .Take(take)
            .ToListAsync();
    }

    // Сохраняет изменения проекта.
    public async Task UpdateAsync(ProjectItem item)
    {
        _context.Projects.Update(item);
        await _context.SaveChangesAsync();
    }
}
