using System.Threading.Tasks;
using hwmvc.Models;
using Microsoft.AspNetCore.Mvc;
using hwmvc.Models.Enums;
using hwmvc.Services.Interfaces;

namespace hwmvc.Controllers
{
    public class ComicController : BaseController
    {
        public ComicController(IWebComicsService webComicsService) : base(webComicsService)
        {
        }

        public async Task<IActionResult> Detail(int id, [FromQuery] Direction direction = Direction.NotSet)
        {
            XKCDComic model = TryGetComicFromSession(id);
            
            if(model == null)
                model = await base.WebComicsService.GetComicAsync(id);
            
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