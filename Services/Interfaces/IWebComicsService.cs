using System.Threading.Tasks;
using hwmvc.Models;
using rain_test.Models.Enums;

namespace rain_test.Services.Interfaces
{
    public interface IWebComicsService
    {
         Task<XKCDComic> GetMostRecentComicAsync();

         Task<XKCDComic> GetComicAsync(int number);

         Task<int?> GetNextAvailableNum(int currentNumber, Direction direction, int lastNumber);
    }
}