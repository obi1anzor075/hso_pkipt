using HsoPkipt.Common;
using HsoPkipt.Controllers;
using HsoPkipt.ViewModels.Project;
using Microsoft.AspNetCore.Mvc;

namespace HsoPkipt.Tests;

public class ProjectsControllerTests
{
    [Fact]
    public async Task Index_ReturnsProjectsViewModel()
    {
        var service = new ProjectServiceStub
        {
            PageToReturn = new PagedResult<ProjectItemVM>(
            [
                new ProjectItemVM
                {
                    Id = Guid.NewGuid(),
                    Title = "Project",
                    ShortDescription = "Description"
                }
            ], 1, 1, 8)
        };

        var controller = new ProjectsController(service);

        var result = await controller.Index();

        var view = Assert.IsType<ViewResult>(result);
        var model = Assert.IsType<ProjectsVM>(view.Model);
        Assert.Single(model.ProjectItems);
        Assert.Equal(1, model.PageNumber);
    }

    [Fact]
    public async Task Details_WhenProjectMissing_ReturnsNotFound()
    {
        var controller = new ProjectsController(new ProjectServiceStub());

        var result = await controller.Details(Guid.NewGuid());

        Assert.IsType<NotFoundResult>(result);
    }
}
