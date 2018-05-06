namespace WeatherApp.Views
{
    using System.Collections.ObjectModel;
    using WeatherApp.ViewModels;
    using Xamarin.Forms;
    using Xamarin.Forms.Xaml;

    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CityDetailsPage : ContentPage
    {
        public CityDetailsPage()
        {
            InitializeComponent();
        }

        public CityViewModel ViewModel => BindingContext as CityViewModel;

        protected override void OnBindingContextChanged()
        {
            base.OnBindingContextChanged();
            CityMap.BindingContext = new MapViewModel(new ObservableCollection<CityViewModel> { ViewModel });
        }
    }
}