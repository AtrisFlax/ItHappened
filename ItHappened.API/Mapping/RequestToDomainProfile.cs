using AutoMapper;
using ItHappened.Api.Models.Requests;
using ItHappened.Domain;
using LanguageExt;

namespace ItHappened.Api.Mapping
{
    public class RequestToDomainProfile : Profile
    {
        public RequestToDomainProfile()
        {
            CreateMap<CustomizationSettingsRequest, TrackerCustomizationSettings>()
                .ConstructUsing(request => new TrackerCustomizationSettings(
                        request.IsPhotoRequired,
                        request.IsScaleRequired,
                        request.ScaleMeasurementUnit,
                        request.IsRatingRequired,
                        request.IsGeotagRequired,
                        request.IsCommentRequired,
                        request.IsCustomizationRequired
                    )
                );
            CreateMap<EventRequest, EventCustomParameters>()
                .ConvertUsing(request => EventCustomParameters(request));

            CreateMap<EventFilterDataRequest, EventFilterData>();
        }

        private static EventCustomParameters EventCustomParameters(EventRequest request)
        {
            var photo = request.Photo == null
                ? Option<Photo>.None
                : Option<Photo>.Some(Photo.Create(request.Photo));
            var scale = request.Scale == null ? Option<double>.None : Option<double>.Some(request.Scale.Value);
            var rating = request.Rating == null
                ? Option<double>.None
                : Option<double>.Some(request.Rating.Value);
            var geoTag = request.GeoTag == null
                ? Option<GeoTag>.None
                : Option<GeoTag>.Some(new GeoTag(request.GeoTag.GpsLat, request.GeoTag.GpsLng));
            var comment = request.Comment == null
                ? Option<Comment>.None
                : Option<Comment>.Some(new Comment(request.Comment));
            return new EventCustomParameters(photo, scale, rating, geoTag, comment);
        }
    }
}