using HsoPkipt.Common;
using HsoPkipt.ViewModels.Users;

namespace HsoPkipt.Services.Interfaces;

public interface IUserService
{
    // Возвращает одну страницу пользователей.
    Task<PagedResult<UserItemVM>> GetUsersPageAsync(int page, int pageSize);

    // Готовит данные пользователя для редактирования.
    Task<UpdateUserVM?> GetForUpdateAsync(Guid id);

    // Обновляет пользователя.
    Task<bool> UpdateAsync(Guid id, UpdateUserVM model);

    // Ищет пользователя по id.
    Task<UserItemVM?> GetByIdAsync(Guid id);

    // Удаляет пользователя.
    Task<bool> DeleteAsync(Guid id);
}
