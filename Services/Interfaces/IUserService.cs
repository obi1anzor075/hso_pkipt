using HsoPkipt.Common;
using HsoPkipt.ViewModels.Users;

namespace HsoPkipt.Services.Interfaces;

public interface IUserService
{
    Task<PagedResult<UserItemVM>> GetUsersPageAsync(int page, int pageSize);
    Task<UpdateUserVM?> GetForUpdateAsync(Guid id);
    Task<bool> UpdateAsync(Guid id, UpdateUserVM model);
    Task<UserItemVM?> GetByIdAsync(Guid id);
    Task<bool> DeleteAsync(Guid id);
}