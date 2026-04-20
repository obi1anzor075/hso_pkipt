using HsoPkipt.ViewModels.Profile;

namespace HsoPkipt.Services.Interfaces;

public interface IProfileService
{
    // Возвращает данные профиля пользователя.
    Task<ProfileVM?> GetProfileAsync(Guid userId);

    // Возвращает ближайшие события для профиля.
    Task<List<EventVM>> GetUpcomingEventsAsync();

    // Возвращает заказы пользователя.
    Task<List<OrderVM>> GetOrdersAsync(Guid userId);

    // Обновляет основную информацию профиля.
    Task<bool> UpdateProfileInfoAsync(Guid userId, ProfileInfoVM model);

    // Меняет пароль и возвращает результат операции.
    Task<(bool Succeeded, List<string> Errors)> ChangePasswordAsync(Guid userId, ChangePasswordVM model);

    // Обновляет фото профиля.
    Task<string?> UpdateProfilePhotoAsync(Guid userId, IFormFile? photo, string webRootPath);
}
