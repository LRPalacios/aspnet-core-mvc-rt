using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using hwmvc.Models;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using rain_test.Models.Enums;
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
            var contractResolver = new CamelCasePropertyNamesContractResolver
            {
                NamingStrategy = new SnakeCaseNamingStrategy()
            };
            this.customJsonSettings = new JsonSerializerSettings
            {
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

        public async Task<XKCDComic> GetComicAsync(int number)
        {
            HttpResponseMessage response = await this.httpClient.GetAsync($"{number}/{webComicResourceName}");
            if (!response.IsSuccessStatusCode)
            {
                //TODO: Log the error
                throw new HttpRequestException($"Something went wrong trying to fetch the comic num: {number}");
            }
            var stringResponse = await response.Content.ReadAsStringAsync();
            var comic = JsonConvert.DeserializeObject<XKCDComic>(stringResponse, customJsonSettings);
            return comic;
        }

        public async Task<int?> GetNextAvailableNum(int currentNumber, Direction direction, int lastNumber)
        {
            if(currentNumber <= 1 && direction== Direction.Backwards)
                return null;
            if(currentNumber >= lastNumber && direction == Direction.Forward)
                return null;
            
            int number = (direction == Direction.Backwards)
                         ? currentNumber - 1
                         : currentNumber + 1;
            
            HttpResponseMessage response = await this.httpClient.GetAsync($"{number}/{webComicResourceName}");
            if (response.StatusCode == HttpStatusCode.NotFound)
                return await GetNextAvailableNum(number, direction, lastNumber);
            
            if (!response.IsSuccessStatusCode)
            {
                //TODO: Log the error
                throw new HttpRequestException($"Something went wrong trying to fetch the comic num: {number}");
            }
            return number;
        }
    }
}