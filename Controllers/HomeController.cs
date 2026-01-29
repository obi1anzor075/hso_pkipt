using HsoPkipt.Services.Interfaces;
using HsoPkipt.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace HsoPkipt.Controllers
{
    public class HomeController : Controller
    {
        private readonly INewsService _newsService;

        public HomeController(INewsService newsService)
        {
            _newsService = newsService;
        }

        // GET: Home/Index
        public async Task<IActionResult> Index()
        {
            var latestNews = await _newsService.GetLatestAsync(5);
            return View(latestNews);
        }

        // GET: Home/News
        // public async Task<IActionResult> News(int page = 1, int pageSize = 9)
        // {
        //     // Получаем первые 9 новостей для начальной загрузки
        //     var news = await _newsService.GetPagedAsync(page, pageSize);
        //     return View(news);
        // }

        // // GET: Home/LoadMoreNews
        // [HttpGet]
        // public async Task<IActionResult> LoadMoreNews(int page = 1, int pageSize = 9)
        // {
        //     if (!Request.Headers.ContainsKey("X-Requested-With"))
        //     {
        //         return BadRequest("Invalid request");
        //     }

        //     var news = await _newsService.GetPagedAsync(page, pageSize);
        //     return PartialView("_NewsCardsPartial", news);
        // }

        // // GET: Home/NewsDetails/5
        // public async Task<IActionResult> NewsDetails(Guid id)
        // {
        //     var newsItem = await _newsService.GetByIdAsync(id);

        //     if (newsItem == null)
        //     {
        //         return NotFound();
        //     }

        //     // Увеличиваем счетчик просмотров
        //     await _newsService.IncrementViewCountAsync(id);

        //     return View(newsItem);
        // }

        public IActionResult OurProject()
        {
            return View();
        }

        public IActionResult News()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}