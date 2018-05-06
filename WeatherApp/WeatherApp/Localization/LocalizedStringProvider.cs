namespace WeatherApp.Localization
{
    using System.ComponentModel;
    using WeatherApp.Helpers.AppSettings;

    public class LocalizedStringProvider : INotifyPropertyChanged
    {
        private static LocalizedStringProvider _instance;

        private LocalizedStringProvider()
        {
            SetHandlers();
        }

        public static LocalizedStringProvider Instance => _instance ?? (_instance = new LocalizedStringProvider());

        public event PropertyChangedEventHandler PropertyChanged;

        public string CitiesText => Resx.AppResources.CitiesText;

        public string SettingsText => Resx.AppResources.SettingsText;

        public string FontColorText => Resx.AppResources.FontColorText;

        public string AppLanguageText => Resx.AppResources.AppLanguageText;

        public string FontSizeText => Resx.AppResources.FontSizeText;

        public string LatitudeText => Resx.AppResources.LatitudeText;

        public string LongitudeText => Resx.AppResources.LongitudeText;

        public string MapText => Resx.AppResources.MapText;

        private void SetHandlers()
        {
            Settings.Instance.LocaleChanged += (o, e) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(CitiesText)));
            Settings.Instance.LocaleChanged += (o, e) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(SettingsText)));
            Settings.Instance.LocaleChanged += (o, e) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(FontColorText)));
            Settings.Instance.LocaleChanged += (o, e) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(AppLanguageText)));
            Settings.Instance.LocaleChanged += (o, e) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(FontSizeText)));
            Settings.Instance.LocaleChanged += (o, e) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(LatitudeText)));
            Settings.Instance.LocaleChanged += (o, e) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(LongitudeText)));
            Settings.Instance.LocaleChanged += (o, e) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(MapText)));
        }
    }
}
