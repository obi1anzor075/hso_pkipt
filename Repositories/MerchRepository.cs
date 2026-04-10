using HsoPkipt.Models;
using HsoPkipt.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace HsoPkipt.Repositories;

public class MerchRepository : IMerchRepository
{
    private readonly AppDbContext _context;

    public MerchRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<List<MerchItem>> GetAllAsync()
    {
        return await _context.MerchItems
            .Include(x => x.Tag)
            .ToListAsync();
    }

    public async Task<List<MerchItem>> GetByTagIdAsync(Guid tagId)
    {
        return await _context.MerchItems
            .Include(x => x.Tag)
            .Where(x => x.TagId == tagId)
            .ToListAsync();
    }

    public async Task<MerchItem?> GetByIdAsync(Guid id)
    {
        return await _context.MerchItems
            .Include(x => x.Tag)
            .FirstOrDefaultAsync(x => x.Id == id);
    }

    public async Task CreateAsync(MerchItem item)
    {
        await _context.MerchItems.AddAsync(item);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(MerchItem item)
    {
        _context.MerchItems.Update(item);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(MerchItem item)
    {
        _context.MerchItems.Remove(item);
        await _context.SaveChangesAsync();
    }

    public async Task<List<MerchItem>> SearchAsync(string query)
    {
        return await _context.MerchItems
            .Include(x => x.Tag)
            .Where(x =>
                x.Name.Contains(query) ||
                (x.Description != null && x.Description.Contains(query))
            )
            .ToListAsync();
    }
}
