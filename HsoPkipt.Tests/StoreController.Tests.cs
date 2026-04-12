using HsoPkipt.Controllers;
using Microsoft.AspNetCore.Mvc;

namespace HsoPkipt.Tests;

public class StoreControllerTests
{
    [Fact]
    public void Store_ReturnsView()
    {
        var controller = new StoreController();

        var result = controller.Store();

        Assert.IsType<ViewResult>(result);
    }
}
