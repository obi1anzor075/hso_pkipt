using System.Linq;
using Microsoft.AspNetCore.Mvc;

namespace HsoPkipt.Controllers
{
    // Этот контроллер показывает служебные документы сайта.
    public class DocumentsController : Controller
    {
        // Открывает нужный документ по короткому имени.
        [HttpGet("/Documents")]
        [HttpGet("/Documents/{slug}")]
        public IActionResult Index(string? slug = null)
        {
            slug = (slug ?? string.Empty).ToLower();

            // Здесь перечислены документы, которые разрешено открывать.
            var allowed = new[] { "privacy_policy", "use_data_policy", "user_guide" };

            // Если пришёл неизвестный адрес, уводим пользователя на документ по умолчанию.
            if (!string.IsNullOrWhiteSpace(slug) && !allowed.Contains(slug))
                return RedirectToAction(nameof(Index), new { slug = "privacy_policy" });

            // Передаём выбранный документ во View.
            ViewBag.DocumentSlug = slug;
            return View();
        }
    }
}
