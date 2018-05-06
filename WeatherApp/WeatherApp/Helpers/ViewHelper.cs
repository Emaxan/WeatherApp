using Xamarin.Forms;

namespace WeatherApp.Helpers
{
    public static class ViewHelper
    {
        public static StackOrientation DetermineOrientation(double width, double height)
        {
            if (height > width)
            {
                return StackOrientation.Vertical;
            }
            else
            {
                return StackOrientation.Horizontal;
            }
        }
    }
}
