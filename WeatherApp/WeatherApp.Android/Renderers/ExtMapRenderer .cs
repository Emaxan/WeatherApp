using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.Gms.Maps;
using Android.Gms.Maps.Model;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using WeatherApp.Controls;
using WeatherApp.Droid.Renderers;
using Xamarin.Forms;
using Xamarin.Forms.Maps;
using Xamarin.Forms.Maps.Android;
using Xamarin.Forms.Platform.Android;

[assembly: ExportRenderer(typeof(ExtMap), typeof(ExtMapRenderer))]
namespace WeatherApp.Droid.Renderers
{
    /// <summary>
    /// Renderer for the xamarin map.
    /// Enable user to get a position by taping on the map.
    /// </summary>
    public class ExtMapRenderer : MapRenderer, IOnMapReadyCallback, GoogleMap.IInfoWindowAdapter
    {
        private List<CustomPin> customPins;

        public ExtMapRenderer(Context context) : base(context)
        {
        }

        protected override void OnMapReady(GoogleMap map)
        {
            base.OnMapReady(map);

            NativeMap.MapClick += GoogleMap_MapClick;
            NativeMap.InfoWindowClick += InfoWindow_Click;
            NativeMap.SetInfoWindowAdapter(this);
        }

        protected override void OnElementChanged(ElementChangedEventArgs<Map> e)
        {
            base.OnElementChanged(e);

            if (e.OldElement != null)
            {
                NativeMap.MapClick -= GoogleMap_MapClick;
                NativeMap.InfoWindowClick -= InfoWindow_Click;
            }

            if (e.NewElement != null)
            {
                var formsMap = (ExtMap)e.NewElement;
                customPins = formsMap.CustomPins;
                Control.GetMapAsync(this);
            }
        }

        protected override MarkerOptions CreateMarker(Pin pin)
        {
            var marker = new MarkerOptions();
            marker.SetPosition(new LatLng(pin.Position.Latitude, pin.Position.Longitude));
            marker.SetTitle(pin.Label);
            marker.SetSnippet(pin.Address);
            return marker;
        }

        public Android.Views.View GetInfoWindow(Marker marker)
        {
            return null;
        }

        public Android.Views.View GetInfoContents(Marker marker)
        {
            var inflater = Android.App.Application.Context.GetSystemService(Context.LayoutInflaterService) as LayoutInflater;
            if (inflater != null)
            {
                Android.Views.View view;

                var customPin = GetCustomPin(marker);
                if (customPin == null)
                {
                    throw new Exception("Custom pin not found");
                }

                view = inflater.Inflate(
                    customPin.Id.ToString() == "Xamarin" ?
                        Resource.Layout.XamarinMapInfoWindow :
                        Resource.Layout.MapInfoWindow,
                    null
                );

                var infoTitle = view.FindViewById<TextView> (Resource.Id.InfoWindowTitle);

                if (infoTitle != null)
                {
                    infoTitle.Text = marker.Title;
                    infoTitle.SetSingleLine(false);
                    infoTitle.SetLines(3);
                }

                return view;
            }
            return null;
        }

        private void GoogleMap_MapClick(object sender, GoogleMap.MapClickEventArgs e)
        {
            ((ExtMap)Element).OnTap(new Position(e.Point.Latitude, e.Point.Longitude));
        }

        private void InfoWindow_Click(object sender, GoogleMap.InfoWindowClickEventArgs e)
        {
            var customPin = GetCustomPin(e.Marker);
            if (customPin == null)
            {
                throw new Exception("Custom pin not found");
            }
        }

        private CustomPin GetCustomPin(Marker annotation)
        {
            var position = new Position(annotation.Position.Latitude, annotation.Position.Longitude);
            foreach (var pin in customPins)
            {
                if (pin.Position == position)
                {
                    return pin;
                }
            }

            return null;
        }
    }
}