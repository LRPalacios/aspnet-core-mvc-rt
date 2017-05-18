using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using hwmvc.Models;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using rain_test.Services.Interfaces;

namespace rain_test.Services
{
    public class WebComicsService : IWebComicsService
    {
        private HttpClient httpClient;
        private IConfiguration configuration;
        private string webComicResourceName;
        private JsonSerializerSettings customJsonSettings;

        public WebComicsService(IConfiguration configuration)
        {
            httpClient = new HttpClient();
            string baseUrl = configuration.GetValue<string>("WebComicApi:baseUrl");
            webComicResourceName = configuration.GetValue<string>("WebComicApi:webComicsResourceName");
            if (string.IsNullOrEmpty(baseUrl) || string.IsNullOrEmpty(webComicResourceName))
                throw new InvalidOperationException("WebComicApi is not correctly definied in the app settings");
            httpClient.BaseAddress = new Uri(baseUrl);
            this.configuration = configuration;
            var contractResolver =  new CamelCasePropertyNamesContractResolver{
                NamingStrategy = new SnakeCaseNamingStrategy()
            };
            this.customJsonSettings = new JsonSerializerSettings { 
                ContractResolver = contractResolver  
            };
        }

        public async Task<XKCDComic> GetMostRecentComicAsync()
        {
            HttpResponseMessage response = await this.httpClient.GetAsync(webComicResourceName);
            if (!response.IsSuccessStatusCode)
            {
                //TODO: Log the error
                throw new HttpRequestException("Something went wrong trying to fetch the most recent comic");
            }
            var stringResponse = await response.Content.ReadAsStringAsync();
            var recentComic = JsonConvert.DeserializeObject<XKCDComic>(stringResponse, customJsonSettings);
            return recentComic;
        }
    }
}