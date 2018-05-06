namespace WeatherApp.Models.ResponseObjects
{
    using Newtonsoft.Json;

    public class WeatherSysDetails
    {
        [JsonProperty("country")]
        public string Country { get; set; }
    }
}
