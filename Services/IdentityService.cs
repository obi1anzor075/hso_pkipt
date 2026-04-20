using HsoPkipt.Identity;
using HsoPkipt.Services.Interfaces;
using HsoPkipt.ViewModels.Users;
using Microsoft.AspNetCore.Identity;

namespace HsoPkipt.Services;

// Сервис для входа, выхода и создания пользователей.
public class IdentityService : IIdentityService
{
    // Менеджер входа.
    private readonly SignInManager<AppUser> _signInManager;

    // Менеджер пользователей.
    private readonly UserManager<AppUser> _userManager;

    // Получаем зависимости через конструктор.
    public IdentityService(
        SignInManager<AppUser> signInManager,
        UserManager<AppUser> userManager)
    {
        _signInManager = signInManager;
        _userManager = userManager;
    }

    // Ищет пользователя по id.
    public async Task<AppUser> GetUserByIdAsync(Guid userId)
    {
        return await _userManager.FindByIdAsync(userId.ToString());
    }

    // Ищет пользователя по почте.
    public async Task<AppUser> GetUserByEmailAsync(string email)
    {
        return await _userManager.FindByEmailAsync(email);
    }

    // Создаёт нового пользователя и сразу выдаёт ему роль.
    public async Task<IdentityResult> CreateUserAsync(CreateUserVM model)
    {
        var user = new AppUser
        {
            Email = model.Email,
            UserName = model.Email,
            FirstName = model.FirstName,
            LastName = model.LastName,
            Position = model.Position
        };

        var result = await _userManager.CreateAsync(user, model.Password);

        if (!result.Succeeded)
            return result;

        var role = string.IsNullOrEmpty(model.Role) ? Roles.User : model.Role;
        var roleResult = await _userManager.AddToRoleAsync(user, role);

        if (!roleResult.Succeeded)
            return roleResult;

        return IdentityResult.Success;
    }

    // Пробует авторизовать пользователя.
    public async Task<SignInResult> SignInAsync(string email, string password, bool rememberMe)
    {
        var user = await GetUserByEmailAsync(email);

        if (user is null)
            return SignInResult.Failed;

        return await _signInManager.PasswordSignInAsync(user, password, rememberMe, lockoutOnFailure: false);
    }

    // Завершает текущую сессию.
    public async Task SignOutAsync()
    {
        await _signInManager.SignOutAsync();
    }
}
