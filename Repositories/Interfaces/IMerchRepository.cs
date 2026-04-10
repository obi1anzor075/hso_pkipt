using HsoPkipt.Models;

namespace HsoPkipt.Repositories.Interfaces;

public interface IMerchRepository
{
    Task<List<MerchItem>> GetAllAsync();
    Task<List<MerchItem>> GetByTagIdAsync(Guid tagId);
    Task<MerchItem?> GetByIdAsync(Guid id);

    Task CreateAsync(MerchItem item);
    Task UpdateAsync(MerchItem item);
    Task DeleteAsync(MerchItem item);

    Task<List<MerchItem>> SearchAsync(string query);
}