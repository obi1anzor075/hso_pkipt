using HsoPkipt.Models;

namespace HsoPkipt.Repositories.Interfaces;

public interface IMerchRepository
{
    // Возвращает все товары.
    Task<List<MerchItem>> GetAllAsync();

    // Возвращает товары по категории.
    Task<List<MerchItem>> GetByTagIdAsync(Guid tagId);

    // Ищет товар по id.
    Task<MerchItem?> GetByIdAsync(Guid id);

    // Сохраняет новый товар.
    Task CreateAsync(MerchItem item);

    // Сохраняет изменения товара.
    Task UpdateAsync(MerchItem item);

    // Удаляет товар.
    Task DeleteAsync(MerchItem item);

    // Ищет товары по тексту.
    Task<List<MerchItem>> SearchAsync(string query);
}
