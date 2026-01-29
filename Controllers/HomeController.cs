using HsoPkipt.Mappers;
using HsoPkipt.Models;
using HsoPkipt.Services.Interfaces;
using HsoPkipt.ViewModels.News;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

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

            var viewModel = new NewsIndexVM
            {
                NewsItems = MapToVm(result.Items),

                PageNumber = result.CurrentPage,
                TotalPages = result.TotalPages
            };

            return View(viewModel);
        }

        [HttpGet]
        public async Task<IActionResult> LoadMoreNews(int page, int pageSize)
        {
            var result = await _newsService.GetNewsPageAsync(page, pageSize);

            var itemsVm = result.Items.Select(n => n.ToViewModel());

            return PartialView("_NewsCardsPartial", itemsVm);
        }

        private IEnumerable<NewsItemVM> MapToVm(IEnumerable<NewsItem> items)
        {
            return items.Select(n => new NewsItemVM
            {
                Id = n.Id,
                Title = n.Title,
                ShortDescription = n.ShortDescription,
                ImageUrl = n.ImageUrl,
                CreatedAt = n.CreatedAt
            });
        }

        public async Task<IActionResult> Index()
        {
            var news = await _newsService.GetLatestAsync(5);

            return View(news);
        }

        public IActionResult OurProject()
        {
            return View();
        }

        public IActionResult About()
        {
            return View();
        }
    }
}