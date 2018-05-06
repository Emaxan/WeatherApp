using WeatherApp.Controls;
using Xamarin.Forms;

[assembly: ExportRenderer(typeof(MultiLineLabel), typeof(WeatherApp.Droid.Renderers.CustomMultiLineLabelRenderer))]
namespace WeatherApp.Droid.Renderers
{
    using Android.Content;
    using Xamarin.Forms.Platform.Android;

    public class CustomMultiLineLabelRenderer : LabelRenderer
    {
        public CustomMultiLineLabelRenderer(Context context) : base(context) { }

        protected override void OnElementChanged(ElementChangedEventArgs<Label> e)
        {
            base.OnElementChanged(e);

            MultiLineLabel multiLineLabel = (MultiLineLabel)Element;

            if (multiLineLabel != null && multiLineLabel.Lines != -1)
            {
                Control.SetSingleLine(false);
                Control.SetLines(multiLineLabel.Lines);
            }
        }
    }
}