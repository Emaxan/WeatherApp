namespace WeatherApp.Views
{
    using System;
    using Xamarin.Forms;
    using Xamarin.Forms.Xaml;

    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CityListView : ContentView
    {
        public CityListView()
        {
            InitializeComponent();
        }

        public event EventHandler<ItemTappedEventArgs> ItemTapped
        {
            add => ListViewCities.ItemTapped += value;
            remove => ListViewCities.ItemTapped -= value;
        }
    }
}