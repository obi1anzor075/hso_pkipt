using HsoPkipt.Identity;
using HsoPkipt.Repositories.Interfaces;
using HsoPkipt.Services.Interfaces;
using HsoPkipt.ViewModels.Profile;
using Microsoft.AspNetCore.Identity;

namespace HsoPkipt.Services;

public class ProfileService : IProfileService
{
    private readonly IEventRepository _eventRepository;
    private readonly IOrderRepository _orderRepository;
    private readonly UserManager<AppUser> _userManager;
    private static readonly TimeZoneInfo MoscowTz = GetMoscowTimeZone();

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
    public ProfileService(
        IEventRepository eventRepository,
        IOrderRepository orderRepository,
        UserManager<AppUser> userManager)
    {
        _eventRepository = eventRepository;
        _orderRepository = orderRepository;
        _userManager = userManager;
    }

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

    public async Task<List<EventVM>> GetUpcomingEventsAsync()
    {
        var events = await _eventRepository.GetAllAsync();

        if (events.Count == 0)
            return new List<EventVM>();

        var nowUtc = DateTime.UtcNow;

        var vm = events
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

        return vm;
    }

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

    private static DateTime ToUtc(DateTime date)
    {
        var unspecified = DateTime.SpecifyKind(date, DateTimeKind.Unspecified);
        return TimeZoneInfo.ConvertTimeToUtc(unspecified, MoscowTz);
    }

    private static DateTime ToMoscow(DateTime dateUtc)
    {
        return TimeZoneInfo.ConvertTimeFromUtc(dateUtc, MoscowTz);
    }
}
