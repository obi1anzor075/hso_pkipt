using HsoPkipt.Common;
using HsoPkipt.Controllers;
using HsoPkipt.ViewModels.Home;
using HsoPkipt.ViewModels.News;
using HsoPkipt.ViewModels.Project;
using Microsoft.AspNetCore.Mvc;

namespace HsoPkipt.Tests;

public class HomeControllerTests
{
    [Fact]
    public async Task Index_ReturnsViewWithLatestNewsAndProjects()
    {
        var newsService = new NewsServiceStub
        {
            LatestToReturn =
            [
                new NewsItemVM
                {
                    Id = Guid.NewGuid(),
                    Title = "News",
                    ShortDescription = "Short"
                }
            ]
        };

        var projectService = new ProjectServiceStub
        {
            PageToReturn = new PagedResult<ProjectItemVM>(
            [
                new ProjectItemVM
                {
                    Id = Guid.NewGuid(),
                    Title = "Project",
                    ShortDescription = "Short"
                }
            ], 1, 1, 5)
        };

        var controller = new HomeController(newsService, projectService);

        var result = await controller.Index();

        var view = Assert.IsType<ViewResult>(result);
        var model = Assert.IsType<HomeIndexVM>(view.Model);
        Assert.Single(model.LatestNews);
        Assert.Single(model.LatestProjects);
    }

    [Fact]
    public async Task NewsDetails_WhenNewsNotFound_ReturnsNotFound()
    {
        var controller = new HomeController(new NewsServiceStub(), new ProjectServiceStub());

        var result = await controller.NewsDetails(Guid.NewGuid());

        Assert.IsType<NotFoundResult>(result);
    }
}
