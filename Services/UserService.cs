using HsoPkipt.Common;
using HsoPkipt.Identity;
using HsoPkipt.Services.Interfaces;
using HsoPkipt.ViewModels.Users;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace HsoPkipt.Services;

// Сервис помогает админке работать со списком пользователей.
public class UserService : IUserService
{
    // Менеджер пользователей из Identity.
    private readonly UserManager<AppUser> _userManager;

    // Получаем менеджер через конструктор.
    public UserService(UserManager<AppUser> userManager)
    {
        _userManager = userManager;
    }

    // Собирает одну страницу пользователей.
    public async Task<PagedResult<UserItemVM>> GetUsersPageAsync(int page, int pageSize)
    {
        var query = _userManager.Users;
        var totalCount = await query.CountAsync();

        var users = await query
            .OrderBy(u => u.Email)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        var items = new List<UserItemVM>();

        foreach (var user in users)
        {
            var roles = await _userManager.GetRolesAsync(user);

            items.Add(new UserItemVM
            {
                Id = user.Id,
                Email = user.Email!,
                Role = roles.FirstOrDefault() ?? Roles.User
            });
        }

        return new PagedResult<UserItemVM>(items, totalCount, page, pageSize);
    }

    // Готовит пользователя для формы редактирования.
    public async Task<UpdateUserVM?> GetForUpdateAsync(Guid id)
    {
        var user = await _userManager.FindByIdAsync(id.ToString());

        if (user == null)
            return null;

        var roles = await _userManager.GetRolesAsync(user);

        return new UpdateUserVM
        {
            Id = user.Id,
            Email = user.Email!,
            Role = roles.FirstOrDefault() ?? Roles.User
        };
    }

    // Обновляет пользователя и его роль.
    public async Task<bool> UpdateAsync(Guid id, UpdateUserVM model)
    {
        var user = await _userManager.FindByIdAsync(id.ToString());

        if (user == null)
            return false;

        user.Email = model.Email;
        user.UserName = model.Email;
        user.FirstName = model.FirstName;
        user.LastName = model.LastName;
        user.Position = model.Position;

        // Сначала снимаем старые роли, потом ставим новую.
        var currentRoles = await _userManager.GetRolesAsync(user);
        await _userManager.RemoveFromRolesAsync(user, currentRoles);
        await _userManager.AddToRoleAsync(user, model.Role);

        var result = await _userManager.UpdateAsync(user);

        return result.Succeeded;
    }

    // Возвращает одного пользователя по id.
    public async Task<UserItemVM?> GetByIdAsync(Guid id)
    {
        var user = await _userManager.FindByIdAsync(id.ToString());

        if (user == null)
            return null;

        var roles = await _userManager.GetRolesAsync(user);

        return new UserItemVM
        {
            Id = user.Id,
            Email = user.Email!,
            Role = roles.FirstOrDefault() ?? Roles.User
        };
    }

    // Удаляет пользователя.
    public async Task<bool> DeleteAsync(Guid id)
    {
        var user = await _userManager.FindByIdAsync(id.ToString());

        if (user == null)
            return false;

        var result = await _userManager.DeleteAsync(user);

        return result.Succeeded;
    }
}
