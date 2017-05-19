using System.Threading.Tasks;
using hwmvc.Models;

namespace rain_test.Services.Interfaces
{
    public interface IWebComicsService
    {
         Task<XKCDComic> GetMostRecentComicAsync();

         Task<XKCDComic> GetComicAsync(int number);
    }
}