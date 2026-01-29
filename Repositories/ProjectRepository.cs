using HsoPkipt.Models;
using HsoPkipt.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace HsoPkipt.Repositories;

public class ProjectRepository : IProjectRepository
{
    private readonly AppDbContext _context;

    public ProjectRepository(AppDbContext context)
    {
        _context = context;
    }
    public async Task<int> CountAsync()
    {
        return await _context.Projects.CountAsync();
    }

    public async Task CreateAsync(ProjectItem item)
    {
        _context.Projects.Add(item);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(ProjectItem item)
    {
        _context.Projects.Remove(item);
        await _context.SaveChangesAsync();
    }

    public async Task<ProjectItem?> GetByIdAsync(Guid id)
    {
        return await _context.Projects.FirstOrDefaultAsync(p => p.Id == id);
    }

    public async Task<List<ProjectItem>> GetLatestAsync(int count = 1)
    {
        return await _context.Projects
            .OrderByDescending(p => p.CreatedAt)
            .Take(count)
            .ToListAsync();
    }

    public async Task<(List<ProjectItem> items, int count)> GetPagedAsync(int pageNumber, int pageSize)
    {
        var query = _context.Projects
            .AsNoTracking()
            .Where(n => n.IsPublished);

        int totalCount = query.Count();

        var items = await query
            .OrderByDescending(p => p.CreatedAt)
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        return (items, totalCount);
    }

    public async Task<List<ProjectItem>> GetRangeAsync(int start, int take)
    {
        return await _context.Projects
            .OrderByDescending(p => p.CreatedAt)
            .Skip(start)
            .Take(take)
            .ToListAsync();
    }

    public async Task UpdateAsync(ProjectItem item)
    {
        _context.Projects.Update(item);
        await _context.SaveChangesAsync();
    }
}
