using System;
using System.Threading.Tasks;
using WeatherApp.Helpers;
using WeatherApp.Helpers.AppSettings;
using WeatherApp.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Maps;

namespace WeatherApp.Views
{
    public partial class CityListPage : ContentPage
    {
        private double _width;
        private double _height;
        private CityListView _citiesListView;
        private ColumnDefinition _cityDetailsColumn;
        private View _currentCityDetails;
        private StackLayout _emptyStackLayout;
        private CitiesMapView _citiesMapView;
        private StackOrientation _orientation;
        private bool _initialized;
        private bool _subscribedLocaleChangeVertical;
        private bool _subscribedLocaleChangeHorizontal;
        private bool _cityDetailsViewShowing;

        public CityListPage()
        {
            InitializeComponent();
        }

        private CityListViewModel ViewModel => BindingContext as CityListViewModel;

        internal void CityDetailsViewClosed()
        {
            _cityDetailsViewShowing = false;
        }

        protected override async void OnSizeAllocated(double width, double height)
        {
            base.OnSizeAllocated(width, height);
            if (Math.Abs(_width - width) > 0.1 || Math.Abs(_height - height) > 0.1)
            {
                _width = width;
                _height = height;

                var newOrientation = ViewHelper.DetermineOrientation(width, height);
                if (_cityDetailsViewShowing && newOrientation == StackOrientation.Horizontal)
                {
                    await CloseCityDetailsView();
                }
                if (_initialized && _orientation != newOrientation)
                {
                    _orientation = newOrientation;
                    SetupLayout(_orientation);
                }
            }
        }

        protected override void OnBindingContextChanged()
        {
            if (!_initialized)
            {
                InitialSetup();
            }
        }

        private void InitialSetup()
        {
            _initialized = true;
            _orientation = StackOrientation.Vertical;
            _citiesListView = new CityListView
            {
                BindingContext = ViewModel,
                HorizontalOptions = LayoutOptions.FillAndExpand
            };

            _emptyStackLayout = new StackLayout();
            _currentCityDetails = _emptyStackLayout;
            _citiesMapView = new CitiesMapView
            {
                BindingContext = new MapViewModel(ViewModel.Cities),
                HorizontalOptions = LayoutOptions.FillAndExpand
            };

            _cityDetailsColumn = new ColumnDefinition
            {
                Width = new GridLength(1, GridUnitType.Star)
            };

            MainGridLayout.Children.Add(_citiesListView, 0, 0);
            Grid.SetRowSpan(_citiesListView, 2);

            SetupLayout(_orientation, false);
        }

        private void SetupLayout(StackOrientation newOrientation, bool subsequentCall = true)
        {
            switch (newOrientation)
            {
                case StackOrientation.Vertical:
                    SetupVerticalLayout(subsequentCall);
                    break;
                case StackOrientation.Horizontal:
                    SetupHorizontalLayout(subsequentCall);
                    break;
                default:
                    SetupVerticalLayout(subsequentCall);
                    break;
            }
        }

        private void SetupVerticalLayout(bool subsequentCall)
        {
            MainGridLayout.Children.Remove(_currentCityDetails);
            MainGridLayout.Children.Remove(_citiesMapView);
            MainGridLayout.ColumnDefinitions.Remove(_cityDetailsColumn);

            _citiesListView.ItemTapped += CityList_ItemTappedVertical;
            if (subsequentCall)
            {
                _citiesListView.ItemTapped -= CityList_ItemTappedHorizontal;
            }
        }

        private void SetupHorizontalLayout(bool subsequentCall)
        {
            MainGridLayout.ColumnDefinitions.Add(_cityDetailsColumn);
            _currentCityDetails = _emptyStackLayout;
            MainGridLayout.Children.Add(_currentCityDetails, 1, 0);
            MainGridLayout.Children.Add(_citiesMapView, 1, 1);

            _citiesListView.ItemTapped += CityList_ItemTappedHorizontal;
            if (subsequentCall)
            {
                _citiesListView.ItemTapped -= CityList_ItemTappedVertical;
            }
        }

        private async void CityList_ItemTappedVertical(object sender, ItemTappedEventArgs e)
        {
            var selectedObject = ((ListView)sender).SelectedItem;
            ((ListView)sender).SelectedItem = null;

            var cityDetailsPage = new CityDetailsPage
            {
                BindingContext = selectedObject
            };

            _cityDetailsViewShowing = true;
            await Navigation.PushAsync(cityDetailsPage);

            if (!_subscribedLocaleChangeVertical)
            {
                Settings.Instance.LocaleChanged += Settings_LocaleChanged_Vertical;
                _subscribedLocaleChangeVertical = true;
            }
        }

        private void CityList_ItemTappedHorizontal(object sender, ItemTappedEventArgs e)
        {
            var selectedCity = ((ListView)sender).SelectedItem as CityViewModel;
            ShowCityDetailsInGrid(selectedCity);
            _citiesMapView.CenterOnPosition(new Position(selectedCity.Latitude, selectedCity.Longitude));

            if (_subscribedLocaleChangeHorizontal) return;
            Settings.Instance.LocaleChanged += Settings_LocaleChanged_Horizontal;
            _subscribedLocaleChangeHorizontal = true;
        }

        private void ShowCityDetailsInGrid(CityViewModel selectedCity)
        {
            MainGridLayout.Children.Remove(_currentCityDetails);
            var newCityDetails = new ScrollView { Content = new CityDetailsView { BindingContext = selectedCity }};
            _currentCityDetails = newCityDetails;
            MainGridLayout.Children.Add(_currentCityDetails, 1, 0);
        }

        private async void Settings_LocaleChanged_Vertical(object sender, EventArgs e)
        {
            await CloseCityDetailsView();
            if (_subscribedLocaleChangeVertical)
            {
                Settings.Instance.LocaleChanged -= Settings_LocaleChanged_Vertical;
                _subscribedLocaleChangeVertical = false;
            }
        }

        private void Settings_LocaleChanged_Horizontal(object sender, EventArgs e)
        {
            MainGridLayout.Children.Remove(_currentCityDetails);
            _currentCityDetails = _emptyStackLayout;
            MainGridLayout.Children.Add(_currentCityDetails, 1, 0);

            if (_subscribedLocaleChangeHorizontal)
            {
                Settings.Instance.LocaleChanged -= Settings_LocaleChanged_Horizontal;
                _subscribedLocaleChangeHorizontal = false;
            }
        }

        private async Task CloseCityDetailsView()
        {
            if (_cityDetailsViewShowing)
            {
                CityDetailsViewClosed();
                await Navigation.PopAsync();
            }
        }
    }
}
