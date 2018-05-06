using WeatherApp.Controls;
using Xamarin.Forms;

[assembly: ExportRenderer(typeof(MultiLineLabel), typeof(WeatherApp.iOS.Renderers.CustomMultiLineLabelRenderer))]
namespace WeatherApp.iOS.Renderers
{
    using Xamarin.Forms.Platform.iOS;

    public class CustomMultiLineLabelRenderer : LabelRenderer
    {
        protected override void OnElementChanged(ElementChangedEventArgs<Label> e)
        {
            base.OnElementChanged(e);

            MultiLineLabel multiLineLabel = (MultiLineLabel)Element;

            if (multiLineLabel != null && multiLineLabel.Lines != -1)
                Control.Lines = multiLineLabel.Lines;
        }
    }
}
