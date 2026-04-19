using HsoPkipt.Identity;
using HsoPkipt.Repositories.Interfaces;
using HsoPkipt.Services.Interfaces;
using HsoPkipt.ViewModels.Profile;
using Microsoft.AspNetCore.Identity;

namespace HsoPkipt.Services;

// Сервис собирает личный кабинет пользователя.
public class ProfileService : IProfileService
{
    // Репозиторий событий нужен для блока ближайших событий.
    private readonly IEventRepository _eventRepository;

    // Репозиторий заказов нужен для истории покупок.
    private readonly IOrderRepository _orderRepository;

    // Менеджер пользователей нужен для чтения и изменения данных аккаунта.
    private readonly UserManager<AppUser> _userManager;

    // Часовой пояс Москвы используем для показа дат человеку.
    private static readonly TimeZoneInfo MoscowTz = GetMoscowTimeZone();

    // Подбираем нужное имя часового пояса для текущей системы.
    private static TimeZoneInfo GetMoscowTimeZone()
    {
        try
        {
            return TimeZoneInfo.FindSystemTimeZoneById("Europe/Moscow");
        }
        catch
        {
            return TimeZoneInfo.FindSystemTimeZoneById("Russian Standard Time");
        }
    }

    // Получаем зависимости через конструктор.
    public ProfileService(
        IEventRepository eventRepository,
        IOrderRepository orderRepository,
        UserManager<AppUser> userManager)
    {
        _eventRepository = eventRepository;
        _orderRepository = orderRepository;
        _userManager = userManager;
    }

    // Собирает всю страницу профиля.
    public async Task<ProfileVM?> GetProfileAsync(Guid userId)
    {
        var user = await _userManager.FindByIdAsync(userId.ToString());

        if (user is null)
            return null;

        var upcomingEvents = await GetUpcomingEventsAsync();
        var orders = await GetOrdersAsync(userId);

        var vm = new ProfileVM
        {
            ProfileInfo = new ProfileInfoVM
            {
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email ?? "",
                PhoneNumber = user.PhoneNumber ?? "",
                Position = user.Position
            },
            ProfilePhoto = new ProfilePhotoVM
            {
                CurrentPhotoUrl = user.PhotoUrl
            },
            UpcomingEvents = upcomingEvents,
            Orders = orders
        };

        return vm;
    }

    // Возвращает будущие события для блока в профиле.
    public async Task<List<EventVM>> GetUpcomingEventsAsync()
    {
        var events = await _eventRepository.GetAllAsync();

        if (events.Count == 0)
            return new List<EventVM>();

        var nowUtc = DateTime.UtcNow;

        return events
            .Where(e => e.EventDate >= nowUtc)
            .OrderBy(e => e.EventDate)
            .Select(e => new EventVM
            {
                Id = e.Id,
                Title = e.Title,
                Date = ToMoscow(e.EventDate),
                Description = e.Description,
                IsPublished = e.IsPublished
            })
            .ToList();
    }

    // Возвращает историю заказов пользователя.
    public async Task<List<OrderVM>> GetOrdersAsync(Guid userId)
    {
        var orders = await _orderRepository.GetByUserIdAsync(userId);

        return orders.Select(x => new OrderVM
        {
            Id = x.Id,
            CreatedAt = ToMoscow(x.CreatedAt),
            RecipientName = x.RecipientName,
            PhoneNumber = x.PhoneNumber,
            DeliveryAddress = x.DeliveryAddress,
            TotalPrice = x.TotalPrice,
            Items = x.Items.Select(item => new OrderItemVM
            {
                Name = item.MerchItemName,
                Price = item.Price,
                Quantity = item.Quantity,
                TotalPrice = item.TotalPrice
            }).ToList()
        }).ToList();
    }

    // Обновляет основную информацию профиля.
    public async Task<bool> UpdateProfileInfoAsync(Guid userId, ProfileInfoVM model)
    {
        var user = await _userManager.FindByIdAsync(userId.ToString());

        if (user is null)
            return false;

        user.Email = model.Email;
        user.UserName = model.Email;
        user.PhoneNumber = model.PhoneNumber;
        user.Position = model.Position;

        var result = await _userManager.UpdateAsync(user);

        return result.Succeeded;
    }

    // Меняет пароль пользователя.
    public async Task<(bool Succeeded, List<string> Errors)> ChangePasswordAsync(Guid userId, ChangePasswordVM model)
    {
        var user = await _userManager.FindByIdAsync(userId.ToString());

        if (user is null)
            return (false, new List<string> { "Пользователь не найден" });

        var result = await _userManager.ChangePasswordAsync(
            user,
            model.CurrentPassword,
            model.NewPassword);

        if (result.Succeeded)
            return (true, new List<string>());

        var errors = result.Errors
            .Select(e => e.Description)
            .ToList();

        return (false, errors);
    }

    // Загружает новое фото профиля и сохраняет ссылку на него.
    public async Task<string?> UpdateProfilePhotoAsync(Guid userId, IFormFile? photo, string webRootPath)
    {
        if (photo is null || photo.Length == 0)
            return null;

        var user = await _userManager.FindByIdAsync(userId.ToString());

        if (user is null)
            return null;

        var uploadsFolder = Path.Combine(webRootPath, "uploads", "profiles");

        if (!Directory.Exists(uploadsFolder))
            Directory.CreateDirectory(uploadsFolder);

        var extension = Path.GetExtension(photo.FileName);
        var fileName = $"{userId}{extension}";
        var filePath = Path.Combine(uploadsFolder, fileName);

        await using (var stream = new FileStream(filePath, FileMode.Create))
        {
            await photo.CopyToAsync(stream);
        }

        var photoUrl = $"/uploads/profiles/{fileName}";
        user.PhotoUrl = photoUrl;

        var result = await _userManager.UpdateAsync(user);

        if (!result.Succeeded)
            return null;

        return photoUrl;
    }

    // Переводит дату из UTC в московское время.
    private static DateTime ToMoscow(DateTime dateUtc)
    {
        return TimeZoneInfo.ConvertTimeFromUtc(dateUtc, MoscowTz);
    }
}
