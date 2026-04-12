using HsoPkipt.Controllers;
using HsoPkipt.ViewModels.Profile;
using Microsoft.AspNetCore.Mvc;

namespace HsoPkipt.Tests;

public class ProfileControllerTests
{
    [Fact]
    public async Task Index_WithoutUserId_ReturnsUnauthorized()
    {
        var controller = new ProfileController(new ProfileServiceStub(), new TestWebHostEnvironment());
        controller.SetUser(isAuthenticated: true);

        var result = await controller.Index();

        Assert.IsType<UnauthorizedResult>(result);
    }

    [Fact]
    public async Task UpdateInfo_WhenServiceFails_SetsErrorAndRedirects()
    {
        var profileService = new ProfileServiceStub
        {
            UpdateInfoResult = false
        };

        var controller = new ProfileController(profileService, new TestWebHostEnvironment());
        controller.SetUser(Guid.NewGuid(), isAuthenticated: true);
        controller.SetTempData();

        var result = await controller.UpdateInfo(new ProfileVM
        {
            ProfileInfo = new ProfileInfoVM()
        });

        var redirect = Assert.IsType<RedirectToActionResult>(result);
        Assert.Equal("Index", redirect.ActionName);
        Assert.Equal("Не удалось обновить профиль", controller.TempData["ProfileError"]);
    }
}
