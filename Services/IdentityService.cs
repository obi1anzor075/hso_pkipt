using HsoPkipt.Identity;
using HsoPkipt.Services.Interfaces;
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
    public async Task<IdentityResult> CreateUserAsync(string email, string userName, string password)
    {
        if (string.IsNullOrWhiteSpace(email) ||
            string.IsNullOrWhiteSpace(userName) ||
            string.IsNullOrWhiteSpace(password))
        {
            return IdentityResult.Failed(new IdentityError
            {
                Code = "InvalidArguments",
                Description = "Email, имя пользователя и пароль обязательны"
            });
        }

        var user = new AppUser
        {
            Email = email,
            UserName = userName
        };

        return await _userManager.CreateAsync(user, password);
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
