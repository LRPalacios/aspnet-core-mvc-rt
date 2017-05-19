using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using hwmvc.Models;
using hwmvc.Services.Interfaces;
using hwmvc.Controllers;

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
            if (model == null)
                return NotFound();
            base.SessionMaxNumber = model.Num;
            await base.SetNextAvailableNumbers(model, checkForNext: false);
            ViewBag.EnterAnimationClass = "fadeIn";
            return View(model);
        }

        public IActionResult NotFoundPage()
        {
            XKCDComic model = new XKCDComic
            {
                Title = "404 Page Not found",
                Alt = "Are you impressed yet?",
                Img = "/images/404.jpg"
            };
            ViewBag.EnterAnimationClass = "fadeIn";
            return View(model);
        }

    }
}
