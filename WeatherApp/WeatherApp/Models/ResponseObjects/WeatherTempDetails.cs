namespace WeatherApp.Models.ResponseObjects
{
    using Newtonsoft.Json;

    public class WeatherTempDetails
    {
        [JsonProperty("temp")]
        public string Temp { get; set; }
        [JsonProperty("humidity")]
        public string Humidity { get; set; }
    }
}
