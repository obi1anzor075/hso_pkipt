using HsoPkipt.Common;
using HsoPkipt.Controllers;
using HsoPkipt.Identity;
using HsoPkipt.Services.Interfaces;
using HsoPkipt.ViewModels;
using HsoPkipt.ViewModels.Merch;
using HsoPkipt.ViewModels.News;
using HsoPkipt.ViewModels.Profile;
using HsoPkipt.ViewModels.Project;
using HsoPkipt.ViewModels.Tags;
using HsoPkipt.ViewModels.Users;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.Extensions.FileProviders;
using System.Security.Claims;

namespace HsoPkipt.Tests;

internal static class ControllerTestSupport
{
    public static void SetUser(this Controller controller, Guid? userId = null, bool isAuthenticated = true)
    {
        var claims = new List<Claim>();

        if (userId.HasValue)
        {
            claims.Add(new Claim(ClaimTypes.NameIdentifier, userId.Value.ToString()));
        }

        var identity = isAuthenticated
            ? new ClaimsIdentity(claims, "TestAuthType")
            : new ClaimsIdentity();

        controller.ControllerContext = new ControllerContext
        {
            HttpContext = new DefaultHttpContext
            {
                User = new ClaimsPrincipal(identity)
            }
        };
    }

    public static void SetTempData(this Controller controller)
    {
        controller.TempData = new TempDataDictionary(
            new DefaultHttpContext(),
            new TestTempDataProvider());
    }
}

internal sealed class TestTempDataProvider : ITempDataProvider
{
    public IDictionary<string, object> LoadTempData(HttpContext context)
    {
        return new Dictionary<string, object>();
    }

    public void SaveTempData(HttpContext context, IDictionary<string, object> values)
    {
    }
}

internal sealed class TestWebHostEnvironment : IWebHostEnvironment
{
    public string ApplicationName { get; set; } = "HsoPkipt.Tests";
    public IFileProvider WebRootFileProvider { get; set; } = new NullFileProvider();
    public string WebRootPath { get; set; } = "wwwroot";
    public string EnvironmentName { get; set; } = "Development";
    public string ContentRootPath { get; set; } = AppContext.BaseDirectory;
    public IFileProvider ContentRootFileProvider { get; set; } = new NullFileProvider();
}

internal sealed class IdentityServiceStub : IIdentityService
{
    public Microsoft.AspNetCore.Identity.SignInResult SignInResultToReturn { get; set; } = Microsoft.AspNetCore.Identity.SignInResult.Success;
    public bool SignOutCalled { get; private set; }

    public Task<AppUser> GetUserByIdAsync(Guid userId)
    {
        return Task.FromResult(new AppUser { Id = userId });
    }

    public Task<AppUser> GetUserByEmailAsync(string email)
    {
        return Task.FromResult(new AppUser { Email = email });
    }

    public Task<Microsoft.AspNetCore.Identity.SignInResult> SignInAsync(string email, string password, bool rememberMe)
    {
        return Task.FromResult(SignInResultToReturn);
    }

    public Task SignOutAsync()
    {
        SignOutCalled = true;
        return Task.CompletedTask;
    }

    public Task<IdentityResult> CreateUserAsync(CreateUserVM model)
    {
        return Task.FromResult(IdentityResult.Success);
    }
}

internal sealed class NewsServiceStub : INewsService
{
    public IReadOnlyList<NewsItemVM> LatestToReturn { get; set; } = [];
    public PagedResult<NewsItemVM> PageToReturn { get; set; } = new([], 0, 1, 1);
    public NewsDetailsVM? DetailsToReturn { get; set; }

    public Task<IReadOnlyList<NewsItemVM>> GetLatestAsync(int count = 5)
    {
        return Task.FromResult(LatestToReturn);
    }

    public Task<PagedResult<NewsItemVM>> GetNewsPageAsync(int pageNumber, int pageSize)
    {
        return Task.FromResult(PageToReturn);
    }

    public Task<NewsDetailsVM?> GetByIdAsync(Guid id)
    {
        return Task.FromResult(DetailsToReturn);
    }

    public Task<UpdateNewsItemVM?> GetForUpdateAsync(Guid id)
    {
        return Task.FromResult<UpdateNewsItemVM?>(null);
    }

    public Task<Guid> CreateAsync(CreateNewsItemVM model)
    {
        return Task.FromResult(Guid.NewGuid());
    }

    public Task<bool> UpdateAsync(Guid id, UpdateNewsItemVM model)
    {
        return Task.FromResult(true);
    }

    public Task<bool> DeleteAsync(Guid id)
    {
        return Task.FromResult(true);
    }

    public Task<int> CountAsync()
    {
        return Task.FromResult(0);
    }
}

internal sealed class ProjectServiceStub : IProjectService
{
    public IReadOnlyList<ProjectItemVM> LatestToReturn { get; set; } = [];
    public PagedResult<ProjectItemVM> PageToReturn { get; set; } = new([], 0, 1, 1);
    public ProjectDetailsVM? DetailsToReturn { get; set; }

    public Task<IReadOnlyList<ProjectItemVM>> GetLatestAsync(int count = 5)
    {
        return Task.FromResult(LatestToReturn);
    }

    public Task<PagedResult<ProjectItemVM>> GetProjectPageAsync(int pageNumber, int pageSize)
    {
        return Task.FromResult(PageToReturn);
    }

    public Task<ProjectDetailsVM?> GetByIdAsync(Guid id)
    {
        return Task.FromResult(DetailsToReturn);
    }

    public Task<UpdateProjectVM?> GetForUpdateAsync(Guid id)
    {
        return Task.FromResult<UpdateProjectVM?>(null);
    }

    public Task<Guid> CreateAsync(CreateProjectVM model)
    {
        return Task.FromResult(Guid.NewGuid());
    }

    public Task<bool> UpdateAsync(Guid id, UpdateProjectVM model)
    {
        return Task.FromResult(true);
    }

    public Task<bool> DeleteAsync(Guid id)
    {
        return Task.FromResult(true);
    }

    public Task<int> CountAsync()
    {
        return Task.FromResult(0);
    }
}

internal sealed class MerchServiceStub : IMerchService
{
    public List<MerchItemVM> AllItemsToReturn { get; set; } = [];
    public List<MerchItemVM> CategoryItemsToReturn { get; set; } = [];
    public List<MerchItemVM> SearchItemsToReturn { get; set; } = [];

    public Task<List<MerchItemVM>> GetAllAsync()
    {
        return Task.FromResult(AllItemsToReturn);
    }

    public Task<List<MerchItemVM>> GetByCategoryAsync(Guid tagId)
    {
        return Task.FromResult(CategoryItemsToReturn);
    }

    public Task<PagedResult<MerchItemVM>> GetPageAsync(int pageNumber, int pageSize)
    {
        return Task.FromResult(new PagedResult<MerchItemVM>([], 0, pageNumber, pageSize));
    }

    public Task<MerchItemVM?> GetByIdAsync(Guid id)
    {
        return Task.FromResult<MerchItemVM?>(null);
    }

    public Task<UpdateMerchItemVM?> GetForUpdateAsync(Guid id)
    {
        return Task.FromResult<UpdateMerchItemVM?>(null);
    }

    public Task CreateAsync(CreateMerchItemVM vm)
    {
        return Task.CompletedTask;
    }

    public Task<bool> UpdateAsync(Guid id, UpdateMerchItemVM vm)
    {
        return Task.FromResult(true);
    }

    public Task<bool> DeleteAsync(Guid id)
    {
        return Task.FromResult(true);
    }

    public Task<List<MerchItemVM>> SearchAsync(string query)
    {
        return Task.FromResult(SearchItemsToReturn);
    }
}

internal sealed class MerchCartServiceStub : IMerchCartService
{
    public MerchCartVM CartToReturn { get; set; } = new();

    public Task<MerchCartVM> GetCartAsync()
    {
        return Task.FromResult(CartToReturn);
    }

    public Task<int> GetItemsCountAsync()
    {
        return Task.FromResult(CartToReturn.ItemsCount);
    }

    public Task AddAsync(Guid merchItemId, int quantity = 1)
    {
        return Task.CompletedTask;
    }

    public Task UpdateQuantityAsync(Guid merchItemId, int quantity)
    {
        return Task.CompletedTask;
    }

    public Task RemoveAsync(Guid merchItemId)
    {
        return Task.CompletedTask;
    }

    public Task ClearAsync()
    {
        return Task.CompletedTask;
    }

    public Task<bool> CheckoutAsync(Guid userId, CheckoutOrderVM model)
    {
        return Task.FromResult(true);
    }
}

internal sealed class ProfileServiceStub : IProfileService
{
    public ProfileVM? ProfileToReturn { get; set; }
    public bool UpdateInfoResult { get; set; } = true;
    public (bool Succeeded, List<string> Errors) ChangePasswordResult { get; set; } = (true, []);
    public string? PhotoUrlToReturn { get; set; }

    public Task<ProfileVM?> GetProfileAsync(Guid userId)
    {
        return Task.FromResult(ProfileToReturn);
    }

    public Task<List<EventVM>> GetUpcomingEventsAsync()
    {
        return Task.FromResult(new List<EventVM>());
    }

    public Task<List<OrderVM>> GetOrdersAsync(Guid userId)
    {
        return Task.FromResult(new List<OrderVM>());
    }

    public Task<bool> UpdateProfileInfoAsync(Guid userId, ProfileInfoVM model)
    {
        return Task.FromResult(UpdateInfoResult);
    }

    public Task<(bool Succeeded, List<string> Errors)> ChangePasswordAsync(Guid userId, ChangePasswordVM model)
    {
        return Task.FromResult(ChangePasswordResult);
    }

    public Task<string?> UpdateProfilePhotoAsync(Guid userId, IFormFile? photo, string webRootPath)
    {
        return Task.FromResult(PhotoUrlToReturn);
    }
}

internal sealed class TagServiceStub : ITagService
{
    public List<TagListItemVM> TagsToReturn { get; set; } = [];
    public TagDetailsVM? DetailsToReturn { get; set; }
    public Exception? CreateException { get; set; }
    public Exception? UpdateException { get; set; }
    public Exception? DeleteException { get; set; }

    public Task<List<TagListItemVM>> GetAllAsync()
    {
        return Task.FromResult(TagsToReturn);
    }

    public Task<TagDetailsVM?> GetByIdAsync(Guid id)
    {
        return Task.FromResult(DetailsToReturn);
    }

    public Task CreateAsync(TagCreateVM vm)
    {
        if (CreateException is not null)
            throw CreateException;

        return Task.CompletedTask;
    }

    public Task UpdateAsync(TagEditVM vm)
    {
        if (UpdateException is not null)
            throw UpdateException;

        return Task.CompletedTask;
    }

    public Task DeleteAsync(Guid id)
    {
        if (DeleteException is not null)
            throw DeleteException;

        return Task.CompletedTask;
    }
}
