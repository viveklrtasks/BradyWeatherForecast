using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BradyWeatherForecast.Model
{
    public class WeatherApiDto
    {
        [JsonProperty(PropertyName = "name")]
        public string Name { get; set; }
        [JsonProperty(PropertyName = "region")] 
        public string Region { get; set; }

        [JsonProperty(PropertyName = "country")] 
        public string Country { get; set; }

        [JsonProperty(PropertyName = "condition")] 
        public string Condition { get; set; }
        [JsonProperty(PropertyName = "icon")] 
        public string Icon { get; set; }
       
        [JsonProperty(PropertyName = "tempreature")]
        public float Tempreature { get; set; }
        [JsonProperty(PropertyName = "wind")]
        public float Wind { get; set; }
        [JsonProperty(PropertyName = "humidity")]
        public float Humidity { get; set; }
        [JsonProperty(PropertyName = "feelsLike")]
        public float FeelsLike { get; set; }

    }
}
