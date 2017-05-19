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

        /* Maybe a ViewModel with the following properties
           is a more separated approach but these will do for now */
        public int? PreviousAvailableNum { get; set; }
        public int? NextAvailableNum { get; set; }

    }
}