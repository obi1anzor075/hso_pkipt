using HsoPkipt.Services.Interfaces;
using HsoPkipt.ViewModels.Home;
using HsoPkipt.ViewModels.News;
using Microsoft.AspNetCore.Mvc;

namespace HsoPkipt.Controllers
{
    public class HomeController : Controller
    {
        private readonly INewsService _newsService;
        private readonly IProjectService _projectService;

        private const int PageSize = 9;

        public HomeController(
            INewsService newsService,
            IProjectService projectService)
        {
            _newsService = newsService;
            _projectService = projectService;
        }

        [HttpGet]
        public async Task<IActionResult> News()
        {
            var result = await _newsService.GetNewsPageAsync(1, PageSize);

            return View(new NewsVM
            {
                NewsItems = result.Items,
                PageNumber = result.CurrentPage,
                TotalPages = result.TotalPages
            });
        }

        [HttpGet]
        public async Task<IActionResult> LoadMoreNews(int page, int pageSize)
        {
            var result = await _newsService.GetNewsPageAsync(page, pageSize);
            return PartialView("_NewsCardsPartial", result.Items);
        }

        public async Task<IActionResult> Index()
        {
            var news = await _newsService.GetLatestAsync(5);
            var projectsPage = await _projectService.GetProjectPageAsync(1, 5);

            var vm = new HomeIndexVM
            {
                LatestNews = news,
                LatestProjects = projectsPage.Items
            };

            return View(vm);
        }

        [HttpGet]
        public async Task<IActionResult> NewsDetails(Guid id)
        {
            var newsItem = await _newsService.GetByIdAsync(id);

            if (newsItem is null)
                return NotFound();

            return View(newsItem);
        }

        public IActionResult About()
        {
            return View();
        }
    }
}