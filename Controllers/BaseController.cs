using System.Threading.Tasks;
using hwmvc.Models;
using Microsoft.AspNetCore.Mvc;
using rain_test.Models.Enums;
using rain_test.Services.Interfaces;

namespace rain_test.Controllers
{
    public abstract class BaseController : Controller
    {
        protected IWebComicsService WebComicsService;

        public BaseController(IWebComicsService webComicsService)
        {
            WebComicsService = webComicsService;
        }

        protected async Task SetNextAvailableNumbers(XKCDComic model)
        {
            model.PreviousAvailableNum = await this.WebComicsService.GetNextAvailableNum(model.Num, Direction.Backwards);
            return;
        }
    }
}