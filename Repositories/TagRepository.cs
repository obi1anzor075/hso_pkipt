using HsoPkipt.Models;
using HsoPkipt.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace HsoPkipt.Repositories;

// Репозиторий работает с категориями товаров.
public class TagRepository : ITagRepository
{
    // Контекст базы данных.
    private readonly AppDbContext _context;

    // Получаем контекст через конструктор.
    public TagRepository(AppDbContext context)
    {
        _context = context;
    }

    // Ищет одну категорию по id.
    public async Task<Tag?> GetByIdAsync(Guid id)
    {
        return await _context.Tags.FirstOrDefaultAsync(t => t.Id == id);
    }

    // Возвращает несколько категорий по списку id.
    public async Task<List<Tag>> GetByIdsAsync(IEnumerable<Guid> ids)
    {
        if (ids is null)
            return new List<Tag>();

        var idList = ids.Distinct().ToList();

        if (idList.Count == 0)
            return new List<Tag>();

        return await _context.Tags
            .Where(t => idList.Contains(t.Id))
            .ToListAsync();
    }

    // Возвращает все категории.
    public async Task<List<Tag>> GetAllAsync()
    {
        return await _context.Tags
            .OrderBy(t => t.Name)
            .ToListAsync();
    }

    // Ищет категорию по названию.
    public async Task<Tag?> GetByNameAsync(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
            return null;

        return await _context.Tags.FirstOrDefaultAsync(t => t.Name == name);
    }

    // Добавляет новую категорию.
    public async Task CreateAsync(Tag tag)
    {
        if (tag is null)
            throw new ArgumentNullException(nameof(tag));

        await _context.Tags.AddAsync(tag);
        await _context.SaveChangesAsync();
    }

    // Сохраняет изменения категории.
    public async Task UpdateAsync(Tag tag)
    {
        if (tag is null)
            throw new ArgumentNullException(nameof(tag));

        _context.Tags.Update(tag);
        await _context.SaveChangesAsync();
    }

    // Удаляет категорию.
    public async Task DeleteAsync(Tag tag)
    {
        if (tag is null)
            throw new ArgumentNullException(nameof(tag));

        _context.Tags.Remove(tag);
        await _context.SaveChangesAsync();
    }
}
