using HsoPkipt.Models;
using HsoPkipt.Repositories.Interfaces;
using HsoPkipt.Services.Interfaces;
using HsoPkipt.ViewModels.Merch;

namespace HsoPkipt.Services;

public class MerchService : IMerchService
{
    private readonly IMerchRepository _repository;

    public MerchService(IMerchRepository repository)
    {
        _repository = repository;
    }

    public async Task<List<MerchItemVM>> GetAllAsync()
    {
        var items = await _repository.GetAllAsync();
        return items.Select(x => new MerchItemVM
        {
            Id = x.Id,
            Name = x.Name,
            Price = x.Price,
            ImageUrl = x.ImageUrl
        }).ToList();
    }

    public async Task<List<MerchItemVM>> GetByCategoryAsync(Guid tagId)
    {
        var items = await _repository.GetByTagIdAsync(tagId);

        return items.Select(x => new MerchItemVM
        {
            Id = x.Id,
            Name = x.Name,
            Price = x.Price,
            ImageUrl = x.ImageUrl
        }).ToList();
    }

    public async Task CreateAsync(CreateMerchItemVM vm)
    {
        var item = new MerchItem(vm.Name, vm.Price, vm.TagId);

        item.Update(vm.Name, vm.Price, vm.TagId, vm.Description, vm.ImageUrl);

        await _repository.CreateAsync(item);
    }

    public async Task<List<MerchItemVM>> SearchAsync(string query)
    {
        var items = await _repository.SearchAsync(query);

        return items.Select(x => new MerchItemVM
        {
            Id = x.Id,
            Name = x.Name,
            Price = x.Price,
            ImageUrl = x.ImageUrl
        }).ToList();
    }
}