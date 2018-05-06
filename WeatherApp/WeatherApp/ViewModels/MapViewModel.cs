namespace WeatherApp.ViewModels
{
    using System;
    using System.Collections.ObjectModel;
    using System.ComponentModel;

    public class MapViewModel : INotifyPropertyChanged
    {
        private ObservableCollection<CityViewModel> _cities;

        public MapViewModel(ObservableCollection<CityViewModel> cities)
        {
            _cities = cities ?? throw new ArgumentNullException(nameof(cities));
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
    }
}
