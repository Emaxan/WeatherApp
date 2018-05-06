namespace WeatherApp.Helpers
{
    using System;
    using Xamarin.Forms.Maps;

    public static class PositionExtensions
    {
        public static double GetDistance(this Position position, Position otherPosition)
        {
            double dX = position.Longitude - otherPosition.Longitude;
            double dY = position.Latitude - otherPosition.Latitude;

            return Math.Sqrt(Math.Pow(dX, 2) + Math.Pow(dY, 2));
        }
    }
}
