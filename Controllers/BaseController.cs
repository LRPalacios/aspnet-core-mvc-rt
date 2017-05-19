using System.Threading.Tasks;
using hwmvc.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using rain_test.Models.Enums;
using rain_test.Services.Interfaces;

namespace rain_test.Controllers
{
    public abstract class BaseController : Controller
    {
        protected IWebComicsService WebComicsService;
        private const string SESSION_MAX_NUMBER_NAME = "_MaxNumber";
        protected int MaxNumber
        {
            get
            {
                /* Avoid requesting each time for the max comic number
                    we set it on the home page but in case is not defined and
                    the session start on a different page than home. We request
                    it here and store it
                 */
                int? number = HttpContext.Session.GetInt32(SESSION_MAX_NUMBER_NAME);
                if (number == null)
                {
                    number = this.WebComicsService.GetMostRecentComicAsync().Result.Num;
                    HttpContext.Session.SetInt32(SESSION_MAX_NUMBER_NAME, number.Value);
                }
                return number.Value;
            }
            set
            {
                HttpContext.Session.SetInt32(SESSION_MAX_NUMBER_NAME, value);
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
            {
                ResesSessionIfNedded(model);
                model.NextAvailableNum = await this.WebComicsService.GetNextAvailableNum(model.Num, Direction.Forward, this.MaxNumber);
            }
            return;
        }

        private void ResesSessionIfNedded(XKCDComic model)
        {
            if (model.Num == this.MaxNumber)
            {
                /* If the user is currently on /comic/MaxKnowNumberByOurSessionVariable
                    let's make sure it really is the MAX Number
                    by clearin to key so that it will be fecth again
                */
                HttpContext.Session.Remove(SESSION_MAX_NUMBER_NAME);
            }
        }
    }
}