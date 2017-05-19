using System.Threading.Tasks;
using hwmvc.Models;
using Microsoft.AspNetCore.Mvc;
using rain_test.Services.Interfaces;

namespace rain_test.Controllers
{
    public class ComicController : BaseController
    {
        public ComicController(IWebComicsService webComicsService) : base(webComicsService)
        {
        }

        public async Task<IActionResult> Detail(int id)
        {
            XKCDComic model = await base.WebComicsService.GetComicAsync(id);
            await base.SetNextAvailableNumbers(model);
            return View(model);
        }
    }
}