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
            //CustomizationSettingsRequest map to => TrackerCustomizationSettings
            CreateMap<CustomizationSettingsRequest, TrackerCustomizationSettings>()
                .ConstructUsing(trackerRequest => new TrackerCustomizationSettings(
                        trackerRequest.IsPhotoRequired,
                        trackerRequest.IsScaleRequired,
                        trackerRequest.ScaleMeasurementUnit,
                        trackerRequest.IsRatingRequired,
                        trackerRequest.IsGeotagRequired,
                        trackerRequest.IsCommentRequired,
                        trackerRequest.AreCustomizationsOptional
                    )
                );

            //EventRequest map to => EventCustomParameters
            CreateMap<EventRequest, EventCustomParameters>()
                .ConvertUsing(trackerRequest => new EventCustomParameters(
                        Photo.Create(Base64Converter.Decode(trackerRequest.Photo)),
                        trackerRequest.Scale.Value,
                        trackerRequest.Rating.Value,
                        new GeoTag(trackerRequest.GeoTag.GpsLat, trackerRequest.GeoTag.GpsLng),
                        new Comment(trackerRequest.Comment)
                    )
                );
        }
    }
}
