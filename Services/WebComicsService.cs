using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using hwmvc.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using rain_test.Models.Enums;
using rain_test.Services.Interfaces;
using rain_test.Utils;

namespace rain_test.Services
{
    public class WebComicsService : IWebComicsService
    {
        private HttpClient httpClient;
        private IConfiguration configuration;
        private string webComicResourceName;
        private JsonSerializerSettings customJsonSettings;
        private readonly ILogger<WebComicsService> logger;

        public WebComicsService(IConfiguration configuration,
                                ILogger<WebComicsService> logger)
        {
            string baseUrl = configuration.GetValue<string>("WebComicApi:baseUrl");
            webComicResourceName = configuration.GetValue<string>("WebComicApi:webComicsResourceName");
            if (ConfigurationNotCorrectlyDefined(baseUrl, webComicResourceName))
                throw new InvalidOperationException("WebComicApi is not correctly definied in the app settings");

            httpClient = new HttpClient
            {
                BaseAddress = new Uri(baseUrl)
            };
            this.configuration = configuration;
            this.customJsonSettings = new CustomJsonSerializerSettings();
            this.logger = logger;
        }

        public async Task<XKCDComic> GetMostRecentComicAsync()
        {
            HttpResponseMessage response = await this.httpClient.GetAsync(webComicResourceName);
            if (!response.IsSuccessStatusCode)
                return await GetResponseNoSuccessResult(response);
            XKCDComic recentComic = await DeserializeComic(response);
            return recentComic;
        }

        public async Task<XKCDComic> GetComicAsync(int number)
        {
            HttpResponseMessage response = await this.httpClient.GetAsync($"{number}/{webComicResourceName}");
            if (!response.IsSuccessStatusCode)
                return await GetResponseNoSuccessResult(response, number);
            var comic = await DeserializeComic(response);
            return comic;
        }

        public async Task<int?> GetNextAvailableNum(int currentNumber, Direction direction, int lastNumber)
        {
            if (currentNumber <= 1 && direction == Direction.Backwards)
                return null;
            if (currentNumber >= lastNumber && direction == Direction.Forward)
                return null;

            int nextPossibleNumber = GetNextPossibleNumber(currentNumber, direction);

            XKCDComic comic = await this.GetComicAsync(nextPossibleNumber);
            if (comic == null)
                return await GetNextAvailableNum(nextPossibleNumber, direction, lastNumber);

            return nextPossibleNumber;
        }

        private bool ConfigurationNotCorrectlyDefined(string baseUrl, string resourceName)
        {
            return string.IsNullOrEmpty(baseUrl) || string.IsNullOrEmpty(resourceName);
        }

        private async Task<XKCDComic> GetResponseNoSuccessResult(HttpResponseMessage response, int? number = null)
        {
            string stringResponse = await response.Content.ReadAsStringAsync();
            if (response.StatusCode == HttpStatusCode.NotFound)
            {
                LogNotFound(number, stringResponse, response.ToString());
                return null;
            }

            LogRequestFailed(number, stringResponse, response.ToString());
            string customExceptionMessage = (number == null)
                                    ? "Something went wrong trying to fetch the most recent comic"
                                    : $"Something went wrong trying to fetch the comic num: {number}";
            throw new HttpRequestException(customExceptionMessage);
        }

        private async Task<XKCDComic> DeserializeComic(HttpResponseMessage response)
        {
            var stringResponse = await response.Content.ReadAsStringAsync();
            var comic = JsonConvert.DeserializeObject<XKCDComic>(stringResponse, customJsonSettings);
            return comic;
        }

        private int GetNextPossibleNumber(int currentNumber, Direction direction)
        {
            return (direction == Direction.Backwards)
                         ? currentNumber - 1
                         : currentNumber + 1;
        }

        private void LogRequestFailed(int? number, string responseBody, string response)
        {
            this.logger.LogError(LoggingEvents.API_REQUEST_COMIC_ERROR,
                            "Request failed. Number: {number}. Response: {response}, ResponseBody: {responseBody}",
                            number,
                            response,
                            responseBody);
        }

        private void LogNotFound(int? number, string responseBody, string response)
        {
            this.logger.LogWarning(LoggingEvents.API_REQUEST_COMIC_NOT_FOUND,
                            "The request to get the comic number: {number}" 
                            + "returned 404. Response: {response} ResponseBody: {responseBody}",
                            number,
                            response,
                            responseBody);
        }

    }
}