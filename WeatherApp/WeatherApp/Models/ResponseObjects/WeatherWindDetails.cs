namespace WeatherApp.Models.ResponseObjects
{
    using Newtonsoft.Json;

    public class WeatherWindDetails
    {
        [JsonProperty("speed")]
        public string Speed { get; set; }
        [JsonProperty("deg")]
        public float Angle { get; set; }
    }
}
