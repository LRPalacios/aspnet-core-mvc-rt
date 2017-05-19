using System.Threading.Tasks;
using hwmvc.Models;
using hwmvc.Models.Enums;

namespace hwmvc.Services.Interfaces
{
    public interface IWebComicsService
    {
         Task<XKCDComic> GetMostRecentComicAsync();

         Task<XKCDComic> GetComicAsync(int number);

         Task<XKCDComic> GetNextAvailableComic(int currentNumber, Direction direction, int lastNumber);
    }
}