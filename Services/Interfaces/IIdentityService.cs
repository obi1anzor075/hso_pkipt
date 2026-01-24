using HsoPkipt.Identity;
using Microsoft.AspNetCore.Identity;

namespace HsoPkipt.Services.Interfaces;

public interface IIdentityService
{
    Task<AppUser> GetUserByIdAsync(Guid userId);
    Task<AppUser> GetUserByEmailAsync(string email);
    Task<SignInResult> SignInAsync(string email, string password, bool rememberMe);
    Task SignOutAsync();
    Task<IdentityResult> CreateUserAsync(string email, string userName, string password);
}
