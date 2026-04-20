using HsoPkipt.Services.Interfaces;
using HsoPkipt.ViewModels.Home;
using HsoPkipt.ViewModels.News;
using Microsoft.AspNetCore.Mvc;

namespace HsoPkipt.Controllers
{
    // Главный контроллер для стартовых страниц сайта.
    public class HomeController : Controller
    {
        // Сервис новостей.
        private readonly INewsService _newsService;

        // Сервис проектов.
        private readonly IProjectService _projectService;

        // Сколько новостей показывать на одной странице.
        private const int PageSize = 9;

        // Получаем зависимости через конструктор.
        public HomeController(
            INewsService newsService,
            IProjectService projectService)
        {
            _newsService = newsService;
            _projectService = projectService;
        }

        // Показывает первую страницу новостей.
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

        // Догружает ещё новости для списка.
        [HttpGet]
        public async Task<IActionResult> LoadMoreNews(int page, int pageSize)
        {
            var result = await _newsService.GetNewsPageAsync(page, pageSize);
            return PartialView("_NewsCardsPartial", result.Items);
        }

        // Собирает данные для главной страницы.
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

        // Показывает одну новость по id.
        [HttpGet]
        public async Task<IActionResult> NewsDetails(Guid id)
        {
            var newsItem = await _newsService.GetByIdAsync(id);

            if (newsItem is null)
                return NotFound();

            return View(newsItem);
        }

        // Открывает страницу "О нас".
        public IActionResult About()
        {
            return View();
        }
    }
}
