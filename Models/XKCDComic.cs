using System.Net;
using System.IO;
using System.Threading.Tasks;

namespace hwmvc.Models
{
    public class XKCDComic
    {
        public string Img { get; set; }
        public string Title { get; set; }
        public string SafeTitle { get; set; }
        public string Alt { get; set; }
        public int Num { get; set; }

    }
}