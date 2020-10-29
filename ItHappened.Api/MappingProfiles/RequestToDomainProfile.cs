using AutoMapper;
using ItHappened.Api.Models.Requests;
using ItHappened.Domain;

namespace ItHappened.Api.MappingProfiles
{
    public class RequestToDomainProfile : Profile
    {
        public RequestToDomainProfile()
        {
            CreateMap<CustomizationSettingsRequest, TrackerCustomizationSettings>().ConstructUsing(trackerRequest =>
                new TrackerCustomizationSettings(
                    trackerRequest.IsPhotoRequired,
                    trackerRequest.IsScaleRequired,
                    trackerRequest.ScaleMeasurementUnit,
                    trackerRequest.IsRatingRequired,
                    trackerRequest.IsGeotagRequired,
                    trackerRequest.IsCommentRequired,
                    trackerRequest.AreCustomizationsOptional
                )
            );
        }
    }
}