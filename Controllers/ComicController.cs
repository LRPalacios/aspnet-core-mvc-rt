using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace rain_test.Controllers
{
    public class ComicController: Controller
    {
        public ComicController()
        {

        }

        public async Task<IActionResult> Index(int Id)
        {
            return View();
        }   
    }
}