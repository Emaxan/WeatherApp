namespace WeatherApp.Models
{
    using System;
    using Newtonsoft.Json;

    public class City
    {
        public City(int weatherid, string name, string country, string description, string smallPhoto, string bigPhoto, double longitude, double latitude)
        {
            Id = weatherid;
            Name = name ?? throw new ArgumentNullException(nameof(name));
            Description = description ?? throw new ArgumentNullException(nameof(description));
            SmallPhoto = smallPhoto ?? throw new ArgumentNullException(nameof(smallPhoto));
            BigPhoto = bigPhoto ?? throw new ArgumentNullException(nameof(bigPhoto));
            Country = country ?? throw new ArgumentNullException(nameof(country));
            Longitude = longitude;
            Latitude = latitude;
        }

        [JsonProperty("weatherid")]
        public int Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("description")]
        public string Description { get; set; }

        [JsonProperty("smallPhoto")]
        public string SmallPhoto { get; set; }

        [JsonProperty("bigPhoto")]
        public string BigPhoto { get; set; }

        [JsonProperty("country")]
        public string Country { get; set; }

        [JsonProperty("longitude")]
        public double Longitude { get; set; }

        [JsonProperty("latitude")]
        public double Latitude { get; set; }
    }
}
