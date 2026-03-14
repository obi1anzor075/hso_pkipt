using HsoPkipt.ViewModels.Profile;

namespace HsoPkipt.Services.Interfaces;

public interface IProfileService
{
    Task<ProfileVM?> GetProfileAsync(Guid userId);
    Task<List<EventVM>> GetUpcomingEventsAsync();
    Task<bool> UpdateProfileInfoAsync(Guid userId, ProfileInfoVM model);
    Task<(bool Succeeded, List<string> Errors)> ChangePasswordAsync(Guid userId, ChangePasswordVM model);
    Task<string?> UpdateProfilePhotoAsync(Guid userId, IFormFile? photo, string webRootPath);
}