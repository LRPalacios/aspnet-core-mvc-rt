using Newtonsoft.Json.Serialization;

namespace hwmvc.Utils
{
    public class CustomContractResolver: CamelCasePropertyNamesContractResolver
    {
        public CustomContractResolver():base()
        {
            NamingStrategy = new SnakeCaseNamingStrategy();
        }
    }
}