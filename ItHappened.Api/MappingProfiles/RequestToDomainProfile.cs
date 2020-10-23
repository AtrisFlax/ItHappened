using AutoMapper;
using ItHappened.Api.Models.Requests;
using ItHappened.Domain;
using LanguageExt;

namespace ItHappened.Api.MappingProfiles
{
    public class RequestToDomainProfile : Profile
    {
        public RequestToDomainProfile()
        {
            CreateMap<CustomizationSettingsRequest, TrackerCustomizationSettings>();
            CreateMap<EventCustomParametersRequest, EventCustomParameters>();
            CreateMap<PhotoRequest, Photo>();
            CreateMap<GeoTagRequest, GeoTag>();
        }
    }
}