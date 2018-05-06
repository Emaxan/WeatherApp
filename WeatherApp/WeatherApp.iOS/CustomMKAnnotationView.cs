namespace WeatherApp.iOS
{
    using MapKit;

    public class CustomMKAnnotationView : MKAnnotationView
    {
        public string Id { get; set; }

        public CustomMKAnnotationView(IMKAnnotation annotation, string id)
            : base(annotation, id)
        {
        }
    }
}