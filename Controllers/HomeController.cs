using HsoPkipt.Services.Interfaces;
using HsoPkipt.ViewModels.News;
using Microsoft.AspNetCore.Mvc;

namespace HsoPkipt.Controllers
{
    public class HomeController : Controller
    {
        private readonly INewsService _newsService;
        private const int PageSize = 9;

        public HomeController(INewsService newsService)
        {
            _newsService = newsService;
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

            return View(news);
        }
    }
}