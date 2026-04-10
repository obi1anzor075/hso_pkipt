using HsoPkipt.Common;
using HsoPkipt.Mappers;
using HsoPkipt.Models;
using HsoPkipt.Repositories.Interfaces;
using HsoPkipt.Services.Interfaces;
using HsoPkipt.ViewModels.Events;

namespace HsoPkipt.Services;

public class EventService : IEventService
{
    private readonly IEventRepository _repository;
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
    public EventService(IEventRepository repository)
    {
        _repository = repository;
    }

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
    public async Task<PagedResult<EventItemVM>> GetPageAsync(int pageNumber, int pageSize)
    {
        if (pageNumber < 1) pageNumber = 1;
        if (pageSize < 1) pageSize = 10;

        var (items, count) = await _repository.GetPagedAsync(pageNumber, pageSize);

        var itemsVM = items.Select(i => i.ToViewModel()).ToList();

        return new PagedResult<EventItemVM>(itemsVM, count, pageNumber, pageSize);
    }

    public async Task<EventDetailsVM?> GetByIdAsync(Guid id)
    {
        var e = await _repository.GetByIdAsync(id);

        if (e == null) return null;

        return new EventDetailsVM
        {
            Id = e.Id,
            Title = e.Title,
            EventDate = ToMoscow(e.EventDate),
            Description = e.Description,
            IsPublished = e.IsPublished
        };
    }

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

    public async Task DeleteAsync(Guid id)
    {
        var entity = await _repository.GetByIdAsync(id);

        if (entity == null)
            throw new Exception("Событие не найдено");

        await _repository.DeleteAsync(entity);
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