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

        private int _maxNumber = 0;
        protected int MaxNumber
        {
            get
            {
                return 1838;
            }
            set
            {
                _maxNumber = value;
            }
        }

        public BaseController(IWebComicsService webComicsService)
        {
            WebComicsService = webComicsService;
        }


        protected async Task SetNextAvailableNumbers(XKCDComic model, bool checkForNext = true)
        {
            model.PreviousAvailableNum = await this.WebComicsService.GetNextAvailableNum(model.Num, Direction.Backwards, this.MaxNumber);
            if (checkForNext)
                model.NextAvailableNum = await this.WebComicsService.GetNextAvailableNum(model.Num, Direction.Forward, this.MaxNumber);
            return;
        }
    }
}