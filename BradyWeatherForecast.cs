using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Net.Http;

namespace BradyWeatherForecast
{
    public static class BradyWeatherForecast
    {
        public const string OperationName = "brady-weather-forecast";
        public const string FunctionRoute = "brady-weather-forecast/{location}";
        public const string Method = "get";

        [FunctionName(OperationName)]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, Method, Route = FunctionRoute)] HttpRequest request,
            string location,
            ILogger log)
        {
            log.LogInformation($"Calling weather api for location - {location}");

            var weatherApiRootUrl = Environment.GetEnvironmentVariable("WeatherApiUrl");
            var weatherApiToken = Environment.GetEnvironmentVariable("WeatherApiToken");
       
            var requestUrl = $"{weatherApiRootUrl}?key={weatherApiToken}&q={location}&aqi=no";

            //CI TEST
            try
            {
                using (var httpClient = new HttpClient())
                {
                    var httpResponse = await httpClient.GetAsync(requestUrl);
                    var result = httpResponse.Content.ReadAsStringAsync();

                    return new JsonResult(result);
                }
            }
            catch (Exception ex)
            {
                log.LogError(ex.StackTrace);
                return new JsonResult(new { Error = ex.Message });
            }
        }
    }
}
