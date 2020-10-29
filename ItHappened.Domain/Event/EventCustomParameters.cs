using LanguageExt;

namespace ItHappened.Domain
{
    public class EventCustomParameters
    {
        public Option<Photo> Photo { get; private set;}
        public Option<double> Scale { get; private set;}
        public Option<double> Rating { get; private set;}
        public Option<GeoTag> GeoTag { get; private set;}
        public Option<Comment> Comment { get; private set;}

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
        
        public EventCustomParameters()
        {
            Photo = Option<Photo>.None;
            Scale = Option<double>.None;
            Rating = Option<double>.None;
            GeoTag = Option<GeoTag>.None;
            Comment = Option<Comment>.None;
        }
    }
}