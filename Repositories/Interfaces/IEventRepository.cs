using HsoPkipt.Models;

namespace HsoPkipt.Repositories.Interfaces;

public interface IEventRepository
{
    // Возвращает все события из базы.
    Task<List<Event>> GetAllAsync();

    // Ищет событие по id.
    Task<Event?> GetByIdAsync(Guid id);

    // Сохраняет новое событие.
    Task AddAsync(Event entity);

    // Сохраняет изменения события.
    Task UpdateAsync(Event entity);

    // Удаляет событие из базы.
    Task DeleteAsync(Event entity);

    // Возвращает страницу событий и общее количество.
    Task<(List<Event> items, int count)> GetPagedAsync(int pageNumber, int pageSize);
}
