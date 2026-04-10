using HsoPkipt.Common;
using HsoPkipt.ViewModels.Merch;

namespace HsoPkipt.Services.Interfaces;

public interface IMerchService
{
    Task<List<MerchItemVM>> GetAllAsync();
    Task<List<MerchItemVM>> GetByCategoryAsync(Guid tagId);
    Task<PagedResult<MerchItemVM>> GetPageAsync(int pageNumber, int pageSize);
    Task<MerchItemVM?> GetByIdAsync(Guid id);
    Task<UpdateMerchItemVM?> GetForUpdateAsync(Guid id);
    Task CreateAsync(CreateMerchItemVM vm);
    Task<bool> UpdateAsync(Guid id, UpdateMerchItemVM vm);
    Task<bool> DeleteAsync(Guid id);
    Task<List<MerchItemVM>> SearchAsync(string query);
}
