namespace WeatherApp.ViewModels
{
    using System.ComponentModel;
    using WeatherApp.Helpers.AppSettings;

    public class SettingsViewModel : INotifyPropertyChanged
    {
        public SettingsViewModel()
        {
            SettingsInstance.PropertyChanged += SettingsInstance_PropertyChanged;
        }

        ~SettingsViewModel()
        {
            SettingsInstance.PropertyChanged -= SettingsInstance_PropertyChanged;
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public Settings SettingsInstance => Settings.Instance;

        public string FontColorText
        {
            get
            {
                return Resx.AppResources.FontColorText;
            }
        }

        public string FontSizeText
        {
            get
            {
                return Resx.AppResources.FontSizeText;
            }
        }

        public string AppLanguageText
        {
            get
            {
                return Resx.AppResources.AppLanguageText;
            }
        }

        private void SettingsInstance_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(Settings.CurrentLocale))
            {
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(FontColorText)));
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(FontSizeText)));
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(AppLanguageText)));
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Settings.CurrentNamedFontColor)));
            }
        }
    }
}
