using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Internal;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using System;
using System.Net;

namespace TestProject1
{
    [TestClass]
    public class BradyWeatherForcastTests
    {
        const string WeatherApiUrl = "http://api.weatherapi.com/v1/current.json";
        const string WeatherApiKey = "41bec3234233493abe384222222603";

        [TestMethod]
        public void TestSuccessFullyReturnsWeatherData()
        {
            var queryStringValue = "London";

            var request = new DefaultHttpRequest(new DefaultHttpContext());

            Environment.SetEnvironmentVariable("WeatherApiUrl", WeatherApiUrl);
            Environment.SetEnvironmentVariable("WeatherApiToken", WeatherApiKey);

            var logger = NullLoggerFactory.Instance.CreateLogger("Null Logger");
            var response = BradyWeatherForecast.BradyWeatherForecast.Run(request, queryStringValue, logger);
            response.Wait();
            Assert.AreEqual(HttpStatusCode.OK, response.Result.StatusCode);
            Assert.IsNotNull(response.Result.Content);
        }

        [TestMethod]
        public void Test_not_returns_weather_data_invalid_location()
        {
            var queryStringValue = "hghghghghgh";

            var request = new DefaultHttpRequest(new DefaultHttpContext());

            Environment.SetEnvironmentVariable("WeatherApiUrl", WeatherApiUrl);
            Environment.SetEnvironmentVariable("WeatherApiToken", WeatherApiKey);

            var logger = NullLoggerFactory.Instance.CreateLogger("Null Logger");
            var response = BradyWeatherForecast.BradyWeatherForecast.Run(request, queryStringValue, logger);
            response.Wait();

            Assert.AreEqual(HttpStatusCode.InternalServerError, response.Result.StatusCode);

        }

        [TestMethod]
        [ExpectedException(typeof(AggregateException))]
        public void Test_returns_exception_if_location_is_empty()
        {
            var queryStringValue = string.Empty;

            var request = new DefaultHttpRequest(new DefaultHttpContext());

            Environment.SetEnvironmentVariable("WeatherApiUrl", WeatherApiUrl);
            Environment.SetEnvironmentVariable("WeatherApiToken", WeatherApiKey);

            var logger = NullLoggerFactory.Instance.CreateLogger("Null Logger");
            var response = BradyWeatherForecast.BradyWeatherForecast.Run(request, queryStringValue, logger);
            response.Wait();
        }
    }
}