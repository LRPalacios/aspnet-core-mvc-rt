using System.Threading.Tasks;
using hwmvc.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using hwmvc.Models.Enums;
using hwmvc.Services.Interfaces;
using hwmvc.Utils;

namespace hwmvc.Controllers
{
    public abstract class BaseController : Controller
    {
        protected IWebComicsService WebComicsService;
        private const string SESSION_MAX_NUMBER_NAME = "_MaxNumber";
        private const string SESSION_PREVIOUS_AVAILABLE_COMIC = "_PreviousAvailableComic";
        private const string SESSION_NEXT_AVAILABLE_COMIC = "_NextAvailableComic";
        protected int SessionMaxNumber
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

        /* Lot of hustle around session I know
         but hey since I'm already launching a request to check the nexts comics
         I might as well store them in session to get from session next time if the users
         is navigating trough the comis. More work I know but better UX for the user */
        protected XKCDComic SessionPreviousAvailableComic
        {
            get
            {
                XKCDComic comic = HttpContext.Session.Get<XKCDComic>(SESSION_PREVIOUS_AVAILABLE_COMIC);
                return comic;
            }
            set
            {
                HttpContext.Session.Set<XKCDComic>(SESSION_PREVIOUS_AVAILABLE_COMIC, value);
            }
        }

        protected XKCDComic SessionNextAvailableComic
        {
            get
            {
                XKCDComic comic = HttpContext.Session.Get<XKCDComic>(SESSION_NEXT_AVAILABLE_COMIC);
                return comic;
            }
            set
            {
                HttpContext.Session.Set<XKCDComic>(SESSION_NEXT_AVAILABLE_COMIC, value);
            }
        }

        public BaseController(IWebComicsService webComicsService)
        {
            WebComicsService = webComicsService;
        }

        protected async Task SetNextAvailableNumbers(XKCDComic model, bool checkForNext = true)
        {
            XKCDComic previousAvailableComic = await this.WebComicsService.GetNextAvailableComic(model.Num, Direction.Backwards, this.SessionMaxNumber);
            this.SessionPreviousAvailableComic = previousAvailableComic;
            model.PreviousAvailableNum = previousAvailableComic?.Num;

            if (checkForNext)
            {
                ResetSessionMaxNumberIfNedded(model);
                XKCDComic nextAvailableComic = await this.WebComicsService.GetNextAvailableComic(model.Num, Direction.Forward, this.SessionMaxNumber);
                this.SessionNextAvailableComic = nextAvailableComic;
                model.NextAvailableNum = nextAvailableComic?.Num;
            }
            return;
        }

        protected XKCDComic TryGetComicFromSession(int number)
        {
            if (this.SessionPreviousAvailableComic != null && this.SessionPreviousAvailableComic.Num == number)
                return this.SessionPreviousAvailableComic;

            if (this.SessionNextAvailableComic != null && this.SessionNextAvailableComic.Num == number)
                return this.SessionNextAvailableComic;

            return null;
        }

        private void ResetSessionMaxNumberIfNedded(XKCDComic model)
        {
            if (model.Num == this.SessionMaxNumber)
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