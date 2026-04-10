using HsoPkipt.ViewModels.Tags;

namespace HsoPkipt.Services.Interfaces;

public interface ITagService
{
    Task<List<TagListItemVM>> GetAllAsync();
    Task<TagDetailsVM?> GetByIdAsync(Guid id);

    Task CreateAsync(TagCreateVM vm);
    Task UpdateAsync(TagEditVM vm);
    Task DeleteAsync(Guid id);
}