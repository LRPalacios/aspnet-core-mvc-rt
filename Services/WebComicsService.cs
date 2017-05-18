using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using hwmvc.Models;
using Microsoft.Extensions.Configuration;
using rain_test.Services.Interfaces;

namespace rain_test.Services
{
    public class WebComicsService : IWebComicsService
    {
        private HttpClient httpClient;
        private IConfiguration configuration;

        public WebComicsService(IConfiguration configuration)
        {
            httpClient = new HttpClient();
            string baseUrl = configuration.GetValue<string>("WebComicApi:baseUrl");
            if (string.IsNullOrEmpty(baseUrl))
                throw new InvalidOperationException("WebComicApi:baseUrl was not found on app settings");
            httpClient.BaseAddress = new Uri(baseUrl);
            this.configuration = configuration;
        }
        public async Task<XKCDComic> GetMostRecentComicAsync()
        {
            return new XKCDComic()
            { //In the skeleton code, we're hard-coding the comic. You'll need to fetch it. Use best practices around using controls and MVC
                Img = "https://imgs.xkcd.com/comics/okeanos.png",
                Title = "Okeanos",
                //NOTE: The Alt text isn't even a part of the model yet. You'll need to add it
            };
        }
    }
}