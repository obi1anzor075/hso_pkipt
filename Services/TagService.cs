using HsoPkipt.Models;
using HsoPkipt.Repositories.Interfaces;
using HsoPkipt.Services.Interfaces;
using HsoPkipt.ViewModels.Tags;

namespace HsoPkipt.Services;

// Сервис управляет категориями товаров.
public class TagService : ITagService
{
    // Репозиторий категорий.
    private readonly ITagRepository _repository;

    // Получаем репозиторий через конструктор.
    public TagService(ITagRepository repository)
    {
        _repository = repository;
    }

    // Возвращает все категории.
    public async Task<List<TagListItemVM>> GetAllAsync()
    {
        var tags = await _repository.GetAllAsync();

        return tags.Select(t => new TagListItemVM
        {
            Id = t.Id,
            Name = t.Name
        }).ToList();
    }

    // Ищет одну категорию по id.
    public async Task<TagDetailsVM?> GetByIdAsync(Guid id)
    {
        var tag = await _repository.GetByIdAsync(id);

        if (tag == null)
            return null;

        return new TagDetailsVM
        {
            Id = tag.Id,
            Name = tag.Name
        };
    }

    // Создаёт новую категорию.
    public async Task CreateAsync(TagCreateVM vm)
    {
        var existing = await _repository.GetByNameAsync(vm.Name);

        if (existing != null)
            throw new Exception("Тег уже существует");

        var tag = new Tag(vm.Name);

        await _repository.CreateAsync(tag);
    }

    // Обновляет название категории.
    public async Task UpdateAsync(TagEditVM vm)
    {
        var tag = await _repository.GetByIdAsync(vm.Id);

        if (tag == null)
            throw new Exception("Тег не найден");

        tag.UpdateTag(vm.Name);

        await _repository.UpdateAsync(tag);
    }

    // Удаляет категорию.
    public async Task DeleteAsync(Guid id)
    {
        var tag = await _repository.GetByIdAsync(id);

        if (tag == null)
            throw new Exception("Тег не найден");

        await _repository.DeleteAsync(tag);
    }
}
