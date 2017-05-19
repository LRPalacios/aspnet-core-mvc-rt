using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using hwmvc.Models;
using rain_test.Services.Interfaces;
using rain_test.Controllers;

namespace hwmvc.Controllers
{

    public class HomeController : BaseController
    {
        public HomeController(IWebComicsService webComicsService) : base(webComicsService)
        {
        }

        public async Task<IActionResult> Index()
        {
            XKCDComic model = await base.WebComicsService.GetMostRecentComicAsync();
            base.MaxNumber = model.Num;
            await base.SetNextAvailableNumbers(model, checkForNext: false);
            return View(model);
        }

        public IActionResult Error()
        {
            return View();
        }
    }
}
