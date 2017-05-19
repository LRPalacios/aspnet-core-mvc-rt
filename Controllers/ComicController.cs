using System.Threading.Tasks;
using hwmvc.Models;
using Microsoft.AspNetCore.Mvc;
using rain_test.Models.Enums;
using rain_test.Services.Interfaces;

namespace rain_test.Controllers
{
    public class ComicController : BaseController
    {
        public ComicController(IWebComicsService webComicsService) : base(webComicsService)
        {
        }

        public async Task<IActionResult> Detail(int id, [FromQuery] Direction direction = Direction.NotSet)
        {
            XKCDComic model = await base.WebComicsService.GetComicAsync(id);
            if (model == null)
                return NotFound();
            await base.SetNextAvailableNumbers(model);
            ViewBag.EnterAnimationClass = GetEnterAnimationClass(direction);
            return View(model);
        }

        private string GetEnterAnimationClass(Direction direction)
        {
            switch (direction)
            {
                case Direction.Forward:
                    return "fadeInRight";
                case Direction.Backwards:
                    return "fadeInLeft";
                case Direction.NotSet:
                    return "fadeIn";
                default:
                    return "fadeIn";
            }
        }
    }
}