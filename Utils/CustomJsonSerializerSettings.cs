using Newtonsoft.Json;

namespace hwmvc.Utils
{
    public class CustomJsonSerializerSettings: JsonSerializerSettings
    {
        public CustomJsonSerializerSettings():base()
        {
            ContractResolver = new CustomContractResolver();
        }
    }
}