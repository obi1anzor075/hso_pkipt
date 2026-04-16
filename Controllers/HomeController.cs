using HsoPkipt.Services.Interfaces;
using HsoPkipt.ViewModels.Home;
using HsoPkipt.ViewModels.News;
using Microsoft.AspNetCore.Mvc;

namespace HsoPkipt.Controllers
{
    public class HomeController : Controller
    {
        // сервис для работы с новостями
        private readonly INewsService _newsService;

        // сервис для работы с проектами
        private readonly IProjectService _projectService;

        // сколько новостей показывать на одной странице
        private const int PageSize = 9;

        // получаем нужные сервисы через зависимости
        public HomeController(
            INewsService newsService,
            IProjectService projectService)
        {
            _newsService = newsService;
            _projectService = projectService;
        }

        [HttpGet]
        // открываем первую страницу со списком новостей
        public async Task<IActionResult> News()
        {
            var result = await _newsService.GetNewsPageAsync(1, PageSize);

            // собираем модель для страницы новостей
            return View(new NewsVM
            {
                NewsItems = result.Items,
                PageNumber = result.CurrentPage,
                TotalPages = result.TotalPages
            });
        }

        [HttpGet]
        // догружаем следующую порцию новостей без полной перезагрузки страницы
        public async Task<IActionResult> LoadMoreNews(int page, int pageSize)
        {
            var result = await _newsService.GetNewsPageAsync(page, pageSize);
            return PartialView("_NewsCardsPartial", result.Items);
        }

        // показываем главную страницу с последними новостями и проектами
        public async Task<IActionResult> Index()
        {
            var news = await _newsService.GetLatestAsync(5);
            var projectsPage = await _projectService.GetProjectPageAsync(1, 5);

            // собираем данные для главной страницы
            var vm = new HomeIndexVM
            {
                LatestNews = news,
                LatestProjects = projectsPage.Items
            };

            return View(vm);
        }

        [HttpGet]
        // открываем подробную страницу конкретной новости
        public async Task<IActionResult> NewsDetails(Guid id)
        {
            var newsItem = await _newsService.GetByIdAsync(id);

            // если новость не нашли, возвращаем 404
            if (newsItem is null)
                return NotFound();

            return View(newsItem);
        }

        // открываем страницу "о нас"
        public IActionResult About()
        {
            return View();
        }
    }
}