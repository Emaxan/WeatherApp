namespace WeatherApp.ViewModels
{
    using System;
    using System.Linq;
    using Models;
    using Helpers;
    using Models.ResponseObjects;

    public class CityViewModel : City
    {
        private const char WindAngleError = 'X';
        private const char WestArrow = (char)8592;
        private const char NorthWestArrow = (char)8598;
        private const char NorthArrow = (char)8594;
        private const char NorthEastArrow = (char)8599;
        private const char EastArrow = (char)8593;
        private const char SouthEastArrow = (char)8600;
        private const char SouthArrow = (char)8595;
        private const char SouthWestArrow = (char)8601;

        private WeatherMainModel _weatherMainModel;

        public CityViewModel(City city) :
            base(city.Id, city.Name, city.Country, city.Description, city.SmallPhoto, city.BigPhoto, city.Longitude, city.Latitude)
        {
        }

        public CityViewModel(int weatherid, string name, string country, string description, string smallPhoto, string bigPhoto, double longitude, double latitude) :
            base(weatherid, name, country, description, smallPhoto, bigPhoto, longitude, latitude)
        {
        }

        public string WeatherMain
        {
            get
            {
                string weatherMain;

                LoadWeather();
                if (_weatherMainModel?.Main == null)
                {
                    weatherMain = Resx.AppResources.WeatherLoadErrorMessage;
                }
                else
                {
                    weatherMain = _weatherMainModel?.Weather?.FirstOrDefault()?.Main is null ?
                        Resx.AppResources.NoWeatherReported :
                        $"{_weatherMainModel.Weather.First().Main}, {_weatherMainModel.Main.Temp}°C";
                }

                return weatherMain;
            }
        }

        public string WindAngle
        {
            get
            {
                char windAngle;
                LoadWeather();
                if (_weatherMainModel?.Wind == null)
                {
                    windAngle = WindAngleError;
                }
                else
                {
                    float angle = _weatherMainModel.Wind.Angle;
                    windAngle = GetArrowSymbol((int)Math.Truncate(angle));
                }

                return windAngle.ToString();
            }
        }

        private char GetArrowSymbol(int angle)
        {
            const int cicrle = 360;
            const int sector = 15;
            int angleSector = (angle % cicrle) / sector + 1;

            switch (angleSector)
            {
                case int i when (i <= 2 || i >= 23):
                    return NorthArrow;
                case int i when (i >= 3 && i <= 4):
                    return NorthEastArrow;
                case int i when (i >= 5 && i <= 8):
                    return EastArrow;
                case int i when (i >= 9 && i <= 10):
                    return SouthEastArrow;
                case int i when (i >= 11 && i <= 14):
                    return SouthArrow;
                case int i when (i >= 15 && i <= 16):
                    return SouthWestArrow;
                case int i when (i >= 17 && i <= 20):
                    return WestArrow;
                case int i when (i >= 21 && i <= 22):
                    return NorthWestArrow;
                default:
                    throw new ArgumentException(nameof(angle));
            }
        }

        private void LoadWeather()
        {
            try
            {
                if (_weatherMainModel is null)
                {
                    _weatherMainModel = WeatherService.GetWeather(Id);
                }
            }
            catch
            {
            }
        }
    }
}
