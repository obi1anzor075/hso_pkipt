using HsoPkipt.Common;
using HsoPkipt.Models;
using HsoPkipt.Repositories.Interfaces;
using HsoPkipt.Services.Interfaces;
using HsoPkipt.ViewModels.Merch;

namespace HsoPkipt.Services;

// Сервис готовит товары мерча для каталога и админки.
public class MerchService : IMerchService
{
    // Репозиторий товаров.
    private readonly IMerchRepository _repository;

    // Получаем репозиторий через конструктор.
    public MerchService(IMerchRepository repository)
    {
        _repository = repository;
    }

    // Возвращает все товары каталога.
    public async Task<List<MerchItemVM>> GetAllAsync()
    {
        var items = await _repository.GetAllAsync();
        return items.Select(ToViewModel).ToList();
    }

    // Возвращает товары одной категории.
    public async Task<List<MerchItemVM>> GetByCategoryAsync(Guid tagId)
    {
        var items = await _repository.GetByTagIdAsync(tagId);

        return items.Select(ToViewModel).ToList();
    }

    // Собирает одну страницу товаров для админки.
    public async Task<PagedResult<MerchItemVM>> GetPageAsync(int pageNumber, int pageSize)
    {
        if (pageNumber < 1)
            pageNumber = 1;

        if (pageSize < 1)
            pageSize = 10;

        var items = await _repository.GetAllAsync();
        var count = items.Count;

        var pageItems = items
            .OrderBy(x => x.Name)
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .Select(ToViewModel)
            .ToList();

        return new PagedResult<MerchItemVM>(pageItems, count, pageNumber, pageSize);
    }

    // Ищет один товар по id.
    public async Task<MerchItemVM?> GetByIdAsync(Guid id)
    {
        var item = await _repository.GetByIdAsync(id);

        return item is null ? null : ToViewModel(item);
    }

    // Готовит модель для формы редактирования товара.
    public async Task<UpdateMerchItemVM?> GetForUpdateAsync(Guid id)
    {
        var item = await _repository.GetByIdAsync(id);

        if (item is null)
            return null;

        return new UpdateMerchItemVM
        {
            Name = item.Name,
            Description = item.Description,
            Price = item.Price,
            ImageUrl = item.ImageUrl,
            TagId = item.TagId
        };
    }

    // Создаёт новый товар.
    public async Task CreateAsync(CreateMerchItemVM vm)
    {
        var item = new MerchItem(vm.Name, vm.Price, vm.TagId);

        item.Update(vm.Name, vm.Price, vm.TagId, vm.Description, vm.ImageUrl);

        await _repository.CreateAsync(item);
    }

    // Обновляет существующий товар.
    public async Task<bool> UpdateAsync(Guid id, UpdateMerchItemVM vm)
    {
        var item = await _repository.GetByIdAsync(id);

        if (item is null)
            return false;

        item.Update(vm.Name, vm.Price, vm.TagId, vm.Description, vm.ImageUrl);
        await _repository.UpdateAsync(item);

        return true;
    }

    // Удаляет товар.
    public async Task<bool> DeleteAsync(Guid id)
    {
        var item = await _repository.GetByIdAsync(id);

        if (item is null)
            return false;

        await _repository.DeleteAsync(item);
        return true;
    }

    // Ищет товары по строке поиска.
    public async Task<List<MerchItemVM>> SearchAsync(string query)
    {
        var items = await _repository.SearchAsync(query);

        return items.Select(ToViewModel).ToList();
    }

    // Переводит сущность товара в view model.
    private static MerchItemVM ToViewModel(MerchItem item)
    {
        return new MerchItemVM
        {
            Id = item.Id,
            Name = item.Name,
            Description = item.Description,
            Price = item.Price,
            ImageUrl = item.ImageUrl,
            TagId = item.TagId,
            TagName = item.Tag?.Name
        };
    }
}
