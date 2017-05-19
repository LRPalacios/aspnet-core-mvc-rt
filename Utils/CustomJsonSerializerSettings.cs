using Newtonsoft.Json;

namespace rain_test.Utils
{
    public class CustomJsonSerializerSettings: JsonSerializerSettings
    {
        public CustomJsonSerializerSettings():base()
        {
            ContractResolver = new CustomContractResolver();
        }
    }
}