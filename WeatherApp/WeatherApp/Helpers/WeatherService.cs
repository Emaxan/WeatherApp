namespace WeatherApp.Helpers
{
    using System.Net.Http;
    using Newtonsoft.Json;
    using WeatherApp.Models.ResponseObjects;

    public static class WeatherService
    {
        private const string _cityWeatherRequestId = "http://api.openweathermap.org/data/2.5/weather?id=";
        private const string _apiKeyPrefix = "&APPID=";
        private const string _apiKey = WeatherApiKeyProvider.Key;
        private const string _settings = "&units=metric";
        private const string _fullApiKey = _apiKeyPrefix + _apiKey;

        private static HttpClient _httpClient = new HttpClient();

        public static WeatherMainModel GetWeather(int cityId)
        {
            var response = _httpClient.GetAsync(_cityWeatherRequestId + cityId + _settings + _fullApiKey).Result;
            var json = response.Content.ReadAsStringAsync().Result;
            var getWeatherModel = JsonConvert.DeserializeObject<WeatherMainModel>(json);
            return getWeatherModel;
        }
    }
}
