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
using BradyWeatherForecast.Model;
using System.Net;
using System.Text;

namespace BradyWeatherForecast
{
    public static class BradyWeatherForecast
    {
        public const string OperationName = "brady-weather-forecast";
        public const string FunctionRoute = "brady-weather-forecast/{location}";
        public const string Method = "get";

        [FunctionName(OperationName)]
        public static async Task<HttpResponseMessage> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, Method, Route = FunctionRoute)] HttpRequest request,
            string location,
            ILogger log)
        {
            log.LogInformation($"Calling weather api for location - {location}");

            var weatherApiRootUrl = Environment.GetEnvironmentVariable("WeatherApiUrl");
            var weatherApiToken = Environment.GetEnvironmentVariable("WeatherApiToken");
       
            var requestUrl = $"{weatherApiRootUrl}?key={weatherApiToken}&q={location}&aqi=no";

            try
            {
                using (var httpClient = new HttpClient())
                {
                    var httpResponse = await httpClient.GetAsync(requestUrl);
                    
                    var weatherData = await httpResponse.Content.ReadAsStringAsync();

                    dynamic deserializedWeatherData = JsonConvert.DeserializeObject(weatherData);

                    var weatherDataToReturn = MapWeatherResult(deserializedWeatherData);

                    return new HttpResponseMessage(HttpStatusCode.OK)
                    {
                        Content = new StringContent(JsonConvert.SerializeObject(weatherDataToReturn), Encoding.UTF8, "application/json")
                    };
                }
            }
            catch (Exception ex)
            {
                log.LogError(ex.StackTrace);
                return new HttpResponseMessage(HttpStatusCode.InternalServerError)
                {
                    Content = new StringContent("Unexpected Error!")
                };
            }
        }

        private static WeatherApiDto MapWeatherResult(dynamic deserializedResult)
        {
           return new WeatherApiDto
            {
                Name = deserializedResult.location.name,
                Country = deserializedResult.location.country,
                Region = deserializedResult.location.region,
                Condition = deserializedResult.current.condition.text,
                Icon = deserializedResult.current.condition.icon,
                Tempreature = deserializedResult.current.temp_c,
                Wind = deserializedResult.current.wind_mph,
                FeelsLike = deserializedResult.current.feelslike_c,
                Humidity = deserializedResult.current.humidity,
            };
        }
    }
}
