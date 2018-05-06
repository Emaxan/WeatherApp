namespace WeatherApp.Helpers.AppSettings
{
    public class LocaleInfo
    {
        private string _name;
        private string _locale;

        public string Name { get => _name; set => _name = value; }
        public string Locale { get => _locale; set => _locale = value; }
    }
}
