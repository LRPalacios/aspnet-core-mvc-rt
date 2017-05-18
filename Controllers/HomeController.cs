using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using hwmvc.Models;
using rain_test.Services.Interfaces;

namespace hwmvc.Controllers
{

    public class HomeController : Controller
    {
        IWebComicsService webComicsService;
        public HomeController(IWebComicsService webComicsService)
        {
            this.webComicsService = webComicsService;
        }

        public async Task<IActionResult> Index()
        {
            XKCDComic model = await this.webComicsService.GetMostRecentComicAsync();
            return View(model);
        }

        public IActionResult Error()
        {
            return View();
        }
    }
}
