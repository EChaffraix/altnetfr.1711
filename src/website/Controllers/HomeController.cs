using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using common;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;

namespace website.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger Logger;
        private readonly string serviceUrl;

        private readonly Func<HttpClient> createClient;

        public HomeController(IOptions<AppSettings> options, ILoggerFactory factory, Func<HttpClient> createClient)
        {
            this.serviceUrl = options.Value.Url;
            this.createClient = createClient;
            Logger = factory.CreateLogger<HomeController>();
            Logger.LogInformation($"Using service at : {serviceUrl}");
        }
        public IActionResult Index()
        {
            var result = Get<VersionMessage>("/ping");
            return View(result);
        }

        protected async Task<TResult> Get<TResult>(string url) where TResult : class
        {
            Logger.LogInformation($"Requesting {url}");
            TResult results = null;
            try
            {
                using (var client = createClient())
                {
                    client.BaseAddress = new Uri(serviceUrl);
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    var response = await client.GetAsync(url);
                    results = await HandleServerAnswer<TResult>(response);
                }
            }
            catch (Exception e)
            {
                Logger.LogError(e, $"Cannot request service {url}");
            }

            return results;
        }
        private async Task<TResult> HandleServerAnswer<TResult>(HttpResponseMessage response)
        {
            TResult results;
            Logger.LogInformation($"Client answered. Status: {response.StatusCode} / {(int)response.StatusCode}");
            var content = await response.Content.ReadAsStringAsync();
            if (response.IsSuccessStatusCode)
            {
                results = JsonConvert.DeserializeObject<TResult>(content);
            }
            else
            {
                Logger.LogError(1, "Unable to post data properly");
                Logger.LogError(content);

                results = default(TResult);

            }
            return results;
        }

    }
}
