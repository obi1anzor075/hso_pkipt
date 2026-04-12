using HsoPkipt.Controllers;
using HsoPkipt.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace HsoPkipt.Tests;

public class AuthControllerTests
{
    [Fact]
    public async Task Login_WhenUserAuthenticated_RedirectsToHome()
    {
        var controller = new AuthController(new IdentityServiceStub());
        controller.SetUser(Guid.NewGuid(), isAuthenticated: true);

        var result = await controller.Login();

        var redirect = Assert.IsType<RedirectToActionResult>(result);
        Assert.Equal("Index", redirect.ActionName);
        Assert.Equal("Home", redirect.ControllerName);
    }

    [Fact]
    public async Task Login_WhenSignInFails_ReturnsViewWithModelError()
    {
        var identityService = new IdentityServiceStub
        {
            SignInResultToReturn = Microsoft.AspNetCore.Identity.SignInResult.Failed
        };

        var controller = new AuthController(identityService);
        var model = new LoginVM
        {
            Email = "test@example.com",
            Password = "password"
        };

        var result = await controller.Login(model);

        var view = Assert.IsType<ViewResult>(result);
        Assert.Same(model, view.Model);
        Assert.False(controller.ModelState.IsValid);
    }
}
