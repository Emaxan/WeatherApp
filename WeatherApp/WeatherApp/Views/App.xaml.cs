namespace WeatherApp.Views
{
    using System.Collections.ObjectModel;
    using WeatherApp.Localization;
    using WeatherApp.ViewModels;
    using Xamarin.Forms;

    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            if (Device.RuntimePlatform == Device.iOS || Device.RuntimePlatform == Device.Android)
            {
                // determine the correct, supported .NET culture
                var ci = DependencyService.Get<ILocalize>().GetCurrentCultureInfo();
                Resx.AppResources.Culture = ci; // set the RESX for resource localization
                DependencyService.Get<ILocalize>().SetLocale(ci); // set the Thread for locale-aware methods
            }

            var page = new TabbedPage();
            var cities = new ObservableCollection<CityViewModel>();
            CityListPage cityListPage = new CityListPage()
            {
                BindingContext = new CityListViewModel(cities)
            };
            var cityListTab = new NavigationPage(cityListPage);
            cityListTab.Popped += (s, e) => { cityListPage.CityDetailsViewClosed(); };
            cityListTab.SetBinding(Page.TitleProperty, new Binding("CitiesText", source: LocalizedStringProvider.Instance));

            page.Children.Add(cityListTab);

            page.Children.Add(new MapPage()
            {
                BindingContext = new MapViewModel(cities)
            });

            page.Children.Add(new SettingsPage()
            {
                BindingContext = new SettingsViewModel()
            });

            MainPage = page;
        }

        protected override void OnStart()
        {
            // Handle when your app starts
        }

        protected override void OnSleep()
        {
            // Handle when your app sleeps
        }

        protected override void OnResume()
        {
            // Handle when your app resumes
        }
    }
}
