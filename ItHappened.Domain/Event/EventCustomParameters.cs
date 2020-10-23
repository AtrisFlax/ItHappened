using LanguageExt;

namespace ItHappened.Domain
{
    public class EventCustomParameters
    {
        public Option<Photo> Photo { get; }
        public Option<double> Scale { get; }
        public Option<double> Rating { get; }
        public Option<GeoTag> GeoTag { get; }
        public Option<Comment> Comment { get; }

        public EventCustomParameters(Option<Photo> photo,
            Option<double> scale,
            Option<double> rating,
            Option<GeoTag> geoTag,
            Option<Comment> comment)
        {
            Photo = photo;
            Scale = scale;
            Rating = rating;
            GeoTag = geoTag;
            Comment = comment;
        }
    }
}