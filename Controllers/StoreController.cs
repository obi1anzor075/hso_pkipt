using HsoPkipt.Services.Interfaces;
using HsoPkipt.ViewModels.News;
using Microsoft.AspNetCore.Mvc;

namespace HsoPkipt.Controllers
{
  public class StoreController : Controller
  {
    public IActionResult Store()
    {
      return View();
    }
  }
}