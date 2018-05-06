namespace WeatherApp.Controls
{
    using System;
    using System.Collections.Generic;
    using Xamarin.Forms.Maps;

    /// <summary>
    /// Extended map:
    /// allow user to tap a point on the map to get the relative position.
    /// </summary>
    public class ExtMap : Map
    {
        public List<CustomPin> CustomPins { get; set; }

        /// <summary>
        /// Event thrown when the user taps on the map
        /// </summary>
        public event EventHandler<MapTapEventArgs> Tapped;

        #region Constructors

        /// <summary>
        /// Default constructor
        /// </summary>
        public ExtMap()
        {
            CustomPins = new List<CustomPin>();
        }

        /// <summary>
        /// Constructor that takes a region
        /// </summary>
        /// <param name="region"></param>
        public ExtMap(MapSpan region)
            : base(region)
        {
            CustomPins = new List<CustomPin>();
        }

        #endregion

        public void OnTap(Position coordinate)
        {
            OnTap(new MapTapEventArgs { Position = coordinate });
        }

        protected virtual void OnTap(MapTapEventArgs e)
        {
            Tapped?.Invoke(this, e);
        }
    }

    public class CustomPin : Pin
    {
    }

    /// <summary>
    /// Event args used with maps, when the user tap on it
    /// </summary>
    public class MapTapEventArgs : EventArgs
    {
        public Position Position { get; set; }
    }
}
