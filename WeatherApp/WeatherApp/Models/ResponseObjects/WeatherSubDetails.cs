namespace WeatherApp.Models.ResponseObjects
{
    using Newtonsoft.Json;

    public class WeatherSubDetails
    {
        [JsonProperty("main")]
        public string Main { get; set; }
        [JsonProperty("description")]
        public string Description { get; set; }
        [JsonProperty("icon")]
        public string Icon { get; set; }
    }
}
