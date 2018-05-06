namespace WeatherApp.ViewModels
{
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.ComponentModel;
    using System.Linq;
    using Helpers;
    using Models;
    using Xamarin.Forms;
    using Xamarin.Forms.Internals;

    public class CityListViewModel : INotifyPropertyChanged
    {
        private const string FileName = "Resources.cities_{0}.json";

        private ObservableCollection<CityViewModel> _cities;
        private Command _loadCitiesCommand;
        private bool _isBusy;

        public CityListViewModel(ObservableCollection<CityViewModel> cities)
        {
            _cities = cities;
            LoadCities();
            Helpers.AppSettings.Settings.Instance.LocaleChanged += (s, e) => { LoadCities(); };
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public ObservableCollection<CityViewModel> Cities
        {
            get => _cities;
            set
            {
                if (_cities == value)
                    return;

                _cities = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Cities)));
            }
        }
        public Command LoadCitiesCommand
        {
            get
            {
                return _loadCitiesCommand ??
                  (_loadCitiesCommand = new Command(ExecuteLoadCitiesCommand, () => !IsBusy));
            }
        }
        public bool IsBusy
        {
            get => _isBusy;
            set
            {
                if (_isBusy == value)
                    return;

                _isBusy = !_isBusy;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(IsBusy)));
            }
        }

        private void ExecuteLoadCitiesCommand()
        {
            if (IsBusy)
                return;

            IsBusy = true;
            LoadCitiesCommand.ChangeCanExecute();

            LoadCities();

            IsBusy = false;
            LoadCitiesCommand.ChangeCanExecute();
        }

        private void LoadCities()
        {
            _cities.Clear();
            var loadedObjects = GetCities();
            loadedObjects.ForEach((obj) => { _cities.Add(obj); });
        }

        private IEnumerable<CityViewModel> GetCities()
        {
            var readObjects = JsonDeserializer.LoadFromJsonByFileName<City>(string.Format(FileName,  Resx.AppResources.FileNamePostfix));
            return readObjects.Select((city) => new CityViewModel(city));
        }
    }
}
