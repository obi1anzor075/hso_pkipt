using HsoPkipt.Controllers.Admin;
using HsoPkipt.ViewModels.Tags;
using Microsoft.AspNetCore.Mvc;

namespace HsoPkipt.Tests;

public class AdminTagControllerTests
{
    [Fact]
    public async Task Tags_ReturnsViewWithTags()
    {
        var service = new TagServiceStub
        {
            TagsToReturn =
            [
                new TagListItemVM
                {
                    Id = Guid.NewGuid(),
                    Name = "Одежда"
                }
            ]
        };

        var controller = new AdminTagController(service);

        var result = await controller.Tags();

        var view = Assert.IsType<ViewResult>(result);
        var model = Assert.IsType<List<TagListItemVM>>(view.Model);
        Assert.Single(model);
    }

    [Fact]
    public async Task CreateTag_WhenServiceThrows_ReturnsViewWithModelError()
    {
        var service = new TagServiceStub
        {
            CreateException = new Exception("Тег уже существует")
        };

        var controller = new AdminTagController(service);
        var model = new TagCreateVM
        {
            Name = "Одежда"
        };

        var result = await controller.CreateTag(model);

        var view = Assert.IsType<ViewResult>(result);
        Assert.Same(model, view.Model);
        Assert.False(controller.ModelState.IsValid);
    }
}
