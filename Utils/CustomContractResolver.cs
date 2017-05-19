using Newtonsoft.Json.Serialization;

namespace rain_test.Utils
{
    public class CustomContractResolver: CamelCasePropertyNamesContractResolver
    {
        public CustomContractResolver():base()
        {
            NamingStrategy = new SnakeCaseNamingStrategy();
        }
    }
}