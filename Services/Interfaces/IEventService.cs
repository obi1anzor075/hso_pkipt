using HsoPkipt.Common;
using HsoPkipt.ViewModels.Events;

namespace HsoPkipt.Services.Interfaces;

public interface IEventService
{
    // Возвращает список всех событий.
    Task<List<EventItemVM>> GetAllAsync();

    // Ищет одно событие по его идентификатору.
    Task<EventDetailsVM?> GetByIdAsync(Guid id);

    // Создаёт новое событие.
    Task CreateAsync(EventCreateVM vm);

    // Обновляет уже существующее событие.
    Task UpdateAsync(EventEditVM vm);

    // Удаляет событие.
    Task DeleteAsync(Guid id);

    // Возвращает одну страницу событий для списка.
    Task<PagedResult<EventItemVM>> GetPageAsync(int pageNumber, int pageSize);
}
