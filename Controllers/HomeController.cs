using HsoPkipt.Services.Interfaces;
using HsoPkipt.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace HsoPkipt.Controllers
{
    public class HomeController : Controller
    {
        private readonly INewsService _newsService;

        public HomeController(INewsService   newsService)
        {
            _newsService = newsService;
        }
        public async Task<IActionResult> Index()
        {
            var latestNews = await _newsService.GetLatestAsync(5);

            return View(latestNews);
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
