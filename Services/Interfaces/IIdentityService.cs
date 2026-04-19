using HsoPkipt.Identity;
using HsoPkipt.ViewModels.Users;
using Microsoft.AspNetCore.Identity;

namespace HsoPkipt.Services.Interfaces;

public interface IIdentityService
{
    // Ищет пользователя по его id.
    Task<AppUser> GetUserByIdAsync(Guid userId);

    // Ищет пользователя по почте.
    Task<AppUser> GetUserByEmailAsync(string email);

    // Выполняет вход пользователя в систему.
    Task<SignInResult> SignInAsync(string email, string password, bool rememberMe);

    // Выводит текущего пользователя из аккаунта.
    Task SignOutAsync();

    // Создаёт нового пользователя.
    Task<IdentityResult> CreateUserAsync(CreateUserVM model);
}
