using System.Security.Claims;
using HsoPkipt.Services.Interfaces;
using HsoPkipt.ViewModels.Profile;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HsoPkipt.Controllers;

// Контроллер личного кабинета пользователя.
[Authorize]
public class ProfileController : Controller
{
    // Сервис профиля.
    private readonly IProfileService _profileService;

    // Окружение нужно, чтобы знать путь к папке сайта.
    private readonly IWebHostEnvironment _environment;

    // Получаем зависимости через конструктор.
    public ProfileController(
        IProfileService profileService,
        IWebHostEnvironment environment)
    {
        _profileService = profileService;
        _environment = environment;
    }

    // Показывает страницу профиля текущего пользователя.
    [HttpGet]
    public async Task<IActionResult> Index()
    {
        var userIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);

        if (string.IsNullOrEmpty(userIdClaim))
            return Unauthorized();

        var userId = Guid.Parse(userIdClaim);
        var model = await _profileService.GetProfileAsync(userId);

        if (model is null)
            return NotFound();

        return View(model);
    }

    // Обновляет основную информацию профиля.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> UpdateInfo(ProfileVM model)
    {
        var userIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);

        if (string.IsNullOrEmpty(userIdClaim))
            return Unauthorized();

        var userId = Guid.Parse(userIdClaim);
        var result = await _profileService.UpdateProfileInfoAsync(userId, model.ProfileInfo);

        if (!result)
        {
            TempData["ProfileError"] = "Не удалось обновить профиль";
            return RedirectToAction(nameof(Index));
        }

        TempData["ProfileSuccess"] = "Основная информация успешно обновлена";
        return RedirectToAction(nameof(Index));
    }

    // Меняет пароль пользователя.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> ChangePassword(ProfileVM model)
    {
        var userIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);

        if (string.IsNullOrEmpty(userIdClaim))
            return Unauthorized();

        var userId = Guid.Parse(userIdClaim);
        var result = await _profileService.ChangePasswordAsync(userId, model.ChangePassword);

        if (!result.Succeeded)
        {
            TempData["PasswordError"] = string.Join(" ", result.Errors);
            return RedirectToAction(nameof(Index));
        }

        TempData["PasswordSuccess"] = "Пароль успешно изменён";
        return RedirectToAction(nameof(Index));
    }

    // Загружает новую фотографию профиля.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> UploadPhoto(ProfileVM model)
    {
        var userIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);

        if (string.IsNullOrEmpty(userIdClaim))
            return Unauthorized();

        var userId = Guid.Parse(userIdClaim);

        var photoUrl = await _profileService.UpdateProfilePhotoAsync(
            userId,
            model.ProfilePhoto.Photo,
            _environment.WebRootPath);

        if (string.IsNullOrEmpty(photoUrl))
        {
            TempData["PhotoError"] = "Не удалось загрузить фото";
            return RedirectToAction(nameof(Index));
        }

        TempData["PhotoSuccess"] = "Фото профиля успешно обновлено";
        return RedirectToAction(nameof(Index));
    }
}
