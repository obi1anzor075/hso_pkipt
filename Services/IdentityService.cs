using HsoPkipt.Identity;
using HsoPkipt.Services.Interfaces;
using HsoPkipt.ViewModels.Users;
using Microsoft.AspNetCore.Identity;

namespace HsoPkipt.Services;

public class IdentityService : IIdentityService
{
    private readonly SignInManager<AppUser> _signInManager;
    private readonly UserManager<AppUser> _userManager;

    public IdentityService(
        SignInManager<AppUser> signInManager,
        UserManager<AppUser> userManager)
    {
        _signInManager = signInManager;
        _userManager = userManager;
    }

    public async Task<AppUser> GetUserByIdAsync(Guid userId)
    {
        return await _userManager.FindByIdAsync(userId.ToString());
    }

    public async Task<AppUser> GetUserByEmailAsync(string email)
    {
        return await _userManager.FindByEmailAsync(email);
    }
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

    public async Task<SignInResult> SignInAsync(string email, string password, bool rememberMe)
    {
        var user = await GetUserByEmailAsync(email);

        if (user is null)
            return SignInResult.Failed;

        return await _signInManager.PasswordSignInAsync(user, password, rememberMe, lockoutOnFailure: false);
    }

    public async Task SignOutAsync()
    {
        await _signInManager .SignOutAsync();
    }
}
