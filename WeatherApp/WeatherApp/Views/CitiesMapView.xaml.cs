namespace WeatherApp.Views
{
    using System;
    using System.Collections.ObjectModel;
    using System.Linq;
    using Controls;
    using Helpers;
    using ViewModels;
    using Xamarin.Forms;
    using Xamarin.Forms.Internals;
    using Xamarin.Forms.Maps;
    using Xamarin.Forms.Xaml;

    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CitiesMapView : ContentView
    {
        public CitiesMapView()
        {
            InitializeComponent();
            CitiesMap.Tapped += CitiesMap_Tapped;
        }

        private MapViewModel ViewModel => BindingContext as MapViewModel;

        public void CenterOnPosition(Position pos)
        {
            CitiesMap.MoveToRegion(MapSpan.FromCenterAndRadius(pos, Distance.FromKilometers(2)));
        }

        protected override void OnBindingContextChanged()
        {
            base.OnBindingContextChanged();
            if (ViewModel is null) return;
            Load();
            ViewModel.Cities.CollectionChanged += (s, e) => Load();
        }

        private void Load()
        {
            ObservableCollection<CityViewModel> cities = ViewModel.Cities;
            CitiesMap.Pins.Clear();
            CitiesMap.CustomPins.Clear();
            if (cities.Count != 0)
            {
                cities.ForEach(c =>
                {
                    CustomPin pin;
                    switch (Device.RuntimePlatform)
                    {
                        case Device.iOS:
                            pin = new CustomPin()
                            {
                                Label = $"{c.Name}",
                                Address = $"{c.WeatherMain}, {Resx.AppResources.WindDirection}: {c.WindAngle}",
                                Type = PinType.Place,
                                Position = new Position(c.Latitude, c.Longitude)
                            };
                            break;
                        default:
                            pin = new CustomPin()
                            {
                                Label = $"{c.Name}{Environment.NewLine}{Resx.AppResources.WindDirection}: {c.WindAngle}{Environment.NewLine}{c.WeatherMain}",
                                Type = PinType.Place,
                                Position = new Position(c.Latitude, c.Longitude)
                            };
                            break;
                    }

                    CitiesMap.Pins.Add(pin);
                    CitiesMap.CustomPins.Add(pin);
                });
                var city = cities.First();
                CenterOnPosition(new Position(city.Latitude, city.Longitude));
            }
        }

        private void CitiesMap_Tapped(object sender, MapTapEventArgs e)
        {
            Position resultPosition;
            ObservableCollection<CityViewModel> cities = ViewModel.Cities;

            if (cities.Count != 0)
            {
                var pos = e.Position;
                CityViewModel closestCity = cities.First();
                double minDistance = pos.GetDistance(new Position(closestCity.Latitude, closestCity.Longitude));

                foreach (var city in cities)
                {
                    double distance = pos.GetDistance(new Position(city.Latitude, city.Longitude));
                    if ((new Position(closestCity.Latitude, closestCity.Longitude) != new Position(city.Latitude, city.Longitude)) && (distance < minDistance))
                    {
                        closestCity = city;
                        minDistance = distance;
                    }
                }

                resultPosition = new Position(closestCity.Latitude, closestCity.Longitude);
            }
            else
            {
                resultPosition = e.Position;
            }

            CenterOnPosition(resultPosition);
        }
    }
}