using LanguageExt;

namespace ItHappened.Domain
{
    public class TrackerCustomizationSettings
    {
        public TrackerCustomizationSettings(Option<string> scaleMeasurementUnit,
            bool photoIsOptional,
            bool ratingIsOptional,
            bool geoTagIsOptional,
            bool commentIsOptional)
        {
            ScaleMeasurementUnit = scaleMeasurementUnit;
            PhotoIsOptional = photoIsOptional;
            RatingIsOptional = ratingIsOptional;
            GeoTagIsOptional = geoTagIsOptional;
            CommentIsOptional = commentIsOptional;
        }
        
        public Option<string> ScaleMeasurementUnit { get; }
        public bool PhotoIsOptional { get; }
        public bool RatingIsOptional { get; }
        public bool GeoTagIsOptional { get; }
        public bool CommentIsOptional { get; }
    }
}