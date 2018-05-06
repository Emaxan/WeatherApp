using System;
using System.Collections.Generic;
using CoreGraphics;
using MapKit;
using UIKit;
using WeatherApp.Controls;
using WeatherApp.iOS.Renderers;
using Xamarin.Forms;
using Xamarin.Forms.Maps;
using Xamarin.Forms.Maps.iOS;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportRenderer(typeof(ExtMap), typeof(ExtMapRenderer))]
namespace WeatherApp.iOS.Renderers
{
    /// <summary>
    /// Renderer for the xamarin ios map control
    /// </summary>
    public class ExtMapRenderer : MapRenderer
    {
        private readonly UITapGestureRecognizer _tapRecogniser;
        private UIView _customPinView;
        private List<CustomPin> _customPins;

        public ExtMapRenderer()
        {
            _tapRecogniser = new UITapGestureRecognizer(OnTap)
            {
                NumberOfTapsRequired = 1,
                NumberOfTouchesRequired = 1
            };
        }

        protected override void OnElementChanged(ElementChangedEventArgs<View> e)
        {
            base.OnElementChanged(e);

            if (Control != null)
            {
                Control.RemoveGestureRecognizer(_tapRecogniser);
            }

            if (Control != null)
            {
                Control.AddGestureRecognizer(_tapRecogniser);
            }

            if (e.OldElement != null)
            {
                if (Control is MKMapView nativeMap)
                {
                    nativeMap.RemoveAnnotations(nativeMap.Annotations);
                    nativeMap.GetViewForAnnotation = null;
                    nativeMap.DidSelectAnnotationView -= OnDidSelectAnnotationView;
                    nativeMap.DidDeselectAnnotationView -= OnDidDeselectAnnotationView;
                    nativeMap.RemoveGestureRecognizer(_tapRecogniser);
                }
            }

            if (e.NewElement != null)
            {
                var formsMap = (ExtMap)e.NewElement;
                var nativeMap = Control as MKMapView;
                _customPins = formsMap.CustomPins;

                nativeMap.GetViewForAnnotation = GetViewForAnnotation;
                nativeMap.DidSelectAnnotationView += OnDidSelectAnnotationView;
                nativeMap.DidDeselectAnnotationView += OnDidDeselectAnnotationView;
                nativeMap.AddGestureRecognizer(_tapRecogniser);
            }
        }

        private MKAnnotationView GetViewForAnnotation(MKMapView mapView, IMKAnnotation annotation)
        {
            MKAnnotationView annotationView = null;

            if (annotation is MKUserLocation)
                return null;

            var customPin = GetCustomPin(annotation as MKPointAnnotation);
            if (customPin == null)
            {
                throw new Exception("Custom pin not found");
            }            

            annotationView = mapView.DequeueReusableAnnotation(customPin.Id.ToString());
            if (annotationView == null)
            {
                annotationView = new CustomMKAnnotationView(annotation, customPin.Id.ToString())
                {
                    CalloutOffset = new CGPoint(0, 0),
                    Image = UIImage.FromFile("pin.png"),
                    Id = customPin.Id.ToString()
                };
            }
            annotationView.CanShowCallout = true;

            return annotationView;
        }

        void OnDidSelectAnnotationView(object sender, MKAnnotationViewEventArgs e)
        {
            var customView = e.View as CustomMKAnnotationView;
            _customPinView = new UIView();

            if (customView.Id == "Xamarin")
            {
                _customPinView.Frame = new CGRect(0, 0, 200, 84);
                _customPinView.Center = new CGPoint(0, -(e.View.Frame.Height + 75));
                e.View.AddSubview(_customPinView);
            }
        }

        void OnDidDeselectAnnotationView(object sender, MKAnnotationViewEventArgs e)
        {
            if (!e.View.Selected)
            {
                _customPinView.RemoveFromSuperview();
                _customPinView.Dispose();
                _customPinView = null;
            }
        }

        private void OnTap(UITapGestureRecognizer recognizer)
        {
            var cgPoint = recognizer.LocationInView(Control);

            var location = ((MKMapView)Control).ConvertPoint(cgPoint, Control);

            ((ExtMap)Element).OnTap(new Position(location.Latitude, location.Longitude));
        }

        CustomPin GetCustomPin(MKPointAnnotation annotation)
        {
            var position = new Position(annotation.Coordinate.Latitude, annotation.Coordinate.Longitude);
            foreach (var pin in _customPins)
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