namespace WeatherApp.Helpers.AppSettings
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.ComponentModel;
    using System.Globalization;
    using System.Linq;
    using Localization;
    using Xamarin.Forms;

    public sealed class Settings : INotifyPropertyChanged
    {
        private static Settings _instance;
        private const string LocaleFileName = "Resources.locales.json";
        private double _currentFontSize;
        private LocaleInfo _currentLocale;
        private Tuple<Color, string> _currentNamedFontColor;

        private Settings()
        {
            Locales = new ObservableCollection<LocaleInfo>(JsonDeserializer.LoadFromJsonByFileName<LocaleInfo>(LocaleFileName));
            LoadColors();
            FontSizes = new ObservableCollection<double>() { 13, 14, 15, 16 };
            CurrentLocale = Locales.First();
            CurrentNamedFontColor = NamedColors.Skip(1).First();
            CurrentFontSize = FontSizes.First();

            PropertyChanged += Settings_PropertyChanged;
        }

        public static Settings Instance => _instance ?? (_instance = new Settings());

        public event PropertyChangedEventHandler PropertyChanged;

        public event EventHandler LocaleChanged;

        public ObservableCollection<LocaleInfo> Locales { get; private set; }

        public ObservableCollection<double> FontSizes { get; private set; }

        public ObservableCollection<Tuple<Color, string>> NamedColors { get; private set; }

        public LocaleInfo CurrentLocale
        {
            get => _currentLocale;
            set
            {
                _currentLocale = value;
                CultureInfo newCi = new CultureInfo(value.Locale);
                Resx.AppResources.Culture = newCi;
                DependencyService.Get<ILocalize>().SetLocale(newCi);
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(CurrentLocale)));
                LocaleChanged?.Invoke(this, new EventArgs());
            }
        }

        public double CurrentFontSize
        {
            get => _currentFontSize;
            set
            {
                _currentFontSize = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(CurrentFontSize)));
            }
        }

        public Tuple<Color, string> CurrentNamedFontColor
        {
            get => _currentNamedFontColor;
            set
            {
                _currentNamedFontColor = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(CurrentNamedFontColor)));
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(CurrentFontColor)));
            }
        }

        public Color CurrentFontColor
        {
            get => CurrentNamedFontColor.Item1;
            set
            {
                CurrentNamedFontColor = NamedColors.First((t) => t.Item1 == value);
            }
        }

        private void LoadColors()
        {
            var prevCurrentNamedColor = CurrentNamedFontColor;
            CurrentNamedFontColor = null;
            if (NamedColors is null)
            {
                NamedColors = new ObservableCollection<Tuple<Color, string>>();
            }
            NamedColors.Clear();
            var newNamedColors = new List<Tuple<Color, string>>() { new Tuple<Color, string>(Color.Black, Resx.AppResources.ColorBlack), new Tuple<Color, string>(Color.Blue, Resx.AppResources.ColorBlue), new Tuple<Color, string>(Color.Brown, Resx.AppResources.ColorBrown),
                new Tuple<Color, string>( Color.Red, Resx.AppResources.ColorRed), new Tuple<Color, string>(Color.Violet, Resx.AppResources.ColorViolet), new Tuple<Color, string>(Color.DeepPink, Resx.AppResources.ColorPink) };
            foreach (var color in newNamedColors)
            {
                NamedColors.Add(color);
            }

            if (prevCurrentNamedColor != null)
            {
                CurrentNamedFontColor = NamedColors.First(c => c.Item1 == prevCurrentNamedColor.Item1);
            }
        }

        private void Settings_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(CurrentLocale))
            {
                LoadColors();
            }
        }
    }
}
