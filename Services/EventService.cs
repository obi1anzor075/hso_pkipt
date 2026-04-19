using HsoPkipt.Common;
using HsoPkipt.Mappers;
using HsoPkipt.Models;
using HsoPkipt.Repositories.Interfaces;
using HsoPkipt.Services.Interfaces;
using HsoPkipt.ViewModels.Events;

namespace HsoPkipt.Services;

// Сервис собирает данные по событиям и переводит их в удобные модели для сайта.
public class EventService : IEventService
{
    // Репозиторий нужен для работы с базой.
    private readonly IEventRepository _repository;

    // Храним московский часовой пояс отдельно, чтобы не искать его каждый раз.
    private static readonly TimeZoneInfo MoscowTz = GetMoscowTimeZone();

    // Здесь пытаемся найти правильное имя часового пояса для текущей системы.
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

    // Получаем репозиторий через конструктор.
    public EventService(IEventRepository repository)
    {
        _repository = repository;
    }

    // Возвращает все будущие события.
    public async Task<List<EventItemVM>> GetAllAsync()
    {
        var events = await _repository.GetAllAsync();

        return events.Select(e => new EventItemVM
        {
            Id = e.Id,
            Title = e.Title,
            EventDate = ToMoscow(e.EventDate),
            IsPublished = e.IsPublished
        }).ToList();
    }

    // Собирает одну страницу событий.
    public async Task<PagedResult<EventItemVM>> GetPageAsync(int pageNumber, int pageSize)
    {
        if (pageNumber < 1)
            pageNumber = 1;

        if (pageSize < 1)
            pageSize = 10;

        var (items, count) = await _repository.GetPagedAsync(pageNumber, pageSize);
        var itemsVm = items.Select(i => i.ToViewModel()).ToList();

        return new PagedResult<EventItemVM>(itemsVm, count, pageNumber, pageSize);
    }

    // Ищет одно событие по id.
    public async Task<EventDetailsVM?> GetByIdAsync(Guid id)
    {
        var e = await _repository.GetByIdAsync(id);

        if (e == null)
            return null;

        return new EventDetailsVM
        {
            Id = e.Id,
            Title = e.Title,
            EventDate = ToMoscow(e.EventDate),
            Description = e.Description,
            IsPublished = e.IsPublished
        };
    }

    // Создаёт новое событие.
    public async Task CreateAsync(EventCreateVM vm)
    {
        var entity = new Event
        {
            Id = Guid.NewGuid(),
            Title = vm.Title,
            EventDate = ToUtc(vm.EventDate),
            Description = vm.Description,
            IsPublished = true
        };

        await _repository.AddAsync(entity);
    }

    // Обновляет существующее событие.
    public async Task UpdateAsync(EventEditVM vm)
    {
        var entity = await _repository.GetByIdAsync(vm.Id);

        if (entity == null)
            throw new Exception("Событие не найдено");

        entity.Title = vm.Title;
        entity.EventDate = ToUtc(vm.EventDate);
        entity.Description = vm.Description;
        entity.IsPublished = vm.IsPublished;

        await _repository.UpdateAsync(entity);
    }

    // Удаляет событие по id.
    public async Task DeleteAsync(Guid id)
    {
        var entity = await _repository.GetByIdAsync(id);

        if (entity == null)
            throw new Exception("Событие не найдено");

        await _repository.DeleteAsync(entity);
    }

    // Переводит время из Москвы в UTC для хранения в базе.
    private static DateTime ToUtc(DateTime date)
    {
        var unspecified = DateTime.SpecifyKind(date, DateTimeKind.Unspecified);
        return TimeZoneInfo.ConvertTimeToUtc(unspecified, MoscowTz);
    }

    // Переводит время из UTC в московское.
    private static DateTime ToMoscow(DateTime dateUtc)
    {
        return TimeZoneInfo.ConvertTimeFromUtc(dateUtc, MoscowTz);
    }
}
