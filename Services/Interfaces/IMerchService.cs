using HsoPkipt.Common;
using HsoPkipt.ViewModels.Merch;

namespace HsoPkipt.Services.Interfaces;

public interface IMerchService
{
    // Возвращает все товары.
    Task<List<MerchItemVM>> GetAllAsync();

    // Возвращает товары одной категории.
    Task<List<MerchItemVM>> GetByCategoryAsync(Guid tagId);

    // Возвращает одну страницу товаров.
    Task<PagedResult<MerchItemVM>> GetPageAsync(int pageNumber, int pageSize);

    // Ищет товар по id.
    Task<MerchItemVM?> GetByIdAsync(Guid id);

    // Готовит данные товара для формы редактирования.
    Task<UpdateMerchItemVM?> GetForUpdateAsync(Guid id);

    // Создаёт новый товар.
    Task CreateAsync(CreateMerchItemVM vm);

    // Обновляет товар.
    Task<bool> UpdateAsync(Guid id, UpdateMerchItemVM vm);

    // Удаляет товар.
    Task<bool> DeleteAsync(Guid id);

    // Ищет товары по тексту.
    Task<List<MerchItemVM>> SearchAsync(string query);
}
