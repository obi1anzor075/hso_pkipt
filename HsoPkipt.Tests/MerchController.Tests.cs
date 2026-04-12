using HsoPkipt.Controllers;
using HsoPkipt.ViewModels.Merch;
using Microsoft.AspNetCore.Mvc;

namespace HsoPkipt.Tests;

public class MerchControllerTests
{
    [Fact]
    public async Task Category_ReturnsIndexViewWithCategoryItems()
    {
        var merchService = new MerchServiceStub
        {
            CategoryItemsToReturn =
            [
                new MerchItemVM
                {
                    Id = Guid.NewGuid(),
                    Name = "Футболка",
                    Price = 1500
                }
            ]
        };

        var controller = new MerchController(merchService, new MerchCartServiceStub());
        controller.SetUser(isAuthenticated: false);

        var result = await controller.Category(Guid.NewGuid());

        var view = Assert.IsType<ViewResult>(result);
        Assert.Equal("Index", view.ViewName);

        var model = Assert.IsType<MerchCatalogVM>(view.Model);
        Assert.Single(model.Items);
    }

    [Fact]
    public async Task Checkout_WhenModelInvalid_ReturnsCartView()
    {
        var controller = new MerchController(new MerchServiceStub(), new MerchCartServiceStub());
        controller.SetUser(isAuthenticated: false);
        controller.ModelState.AddModelError("Checkout.RecipientName", "Required");

        var result = await controller.Checkout(new MerchCartPageVM());

        var view = Assert.IsType<ViewResult>(result);
        Assert.Equal("Cart", view.ViewName);
        Assert.IsType<MerchCartPageVM>(view.Model);
    }
}
