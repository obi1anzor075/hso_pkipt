using HsoPkipt.Controllers.Admin;
using Microsoft.AspNetCore.Mvc;

namespace HsoPkipt.Tests;

public class AdminControllerTests
{
    [Fact]
    public void Index_ReturnsView()
    {
        var controller = new AdminController();

        var result = controller.Index();

        Assert.IsType<ViewResult>(result);
    }

    [Fact]
    public void Tags_ReturnsView()
    {
        var controller = new AdminController();

        var result = controller.Tags();

        Assert.IsType<ViewResult>(result);
    }
}
