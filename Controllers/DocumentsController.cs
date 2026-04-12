using System.Linq;
using Microsoft.AspNetCore.Mvc;

namespace HsoPkipt.Controllers
{
    public class DocumentsController : Controller
    {
        [HttpGet("/Documents")]
        [HttpGet("/Documents/{slug}")]
        public IActionResult Index(string? slug = null)
        {
            slug = (slug ?? string.Empty).ToLower();

            var allowed = new[] { "privacy_policy", "use_data_policy", "user_guide" };

            if (!string.IsNullOrWhiteSpace(slug) && !allowed.Contains(slug))
                return RedirectToAction(nameof(Index), new { slug = "privacy_policy" });

            ViewBag.DocumentSlug = slug;
            return View();
        }
    }
}