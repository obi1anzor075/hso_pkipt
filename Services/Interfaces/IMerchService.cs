using HsoPkipt.ViewModels.Merch;

namespace HsoPkipt.Services.Interfaces;

public interface IMerchService
{
    Task<List<MerchItemVM>> GetAllAsync();
    Task<List<MerchItemVM>> GetByCategoryAsync(Guid tagId);
    Task CreateAsync(CreateMerchItemVM vm);
    Task<List<MerchItemVM>> SearchAsync(string query);
}