using AutoMapper;
using ItHappened.Api.Contracts.Requests;
using ItHappened.Domain;

namespace ItHappened.Api.MappingProfiles
{
    public class RequestToDomainProfile : Profile
    {
        public RequestToDomainProfile()
        {
            CreateMap<CustomizationSettingsRequest, TrackerCustomizationSettings>();
            CreateMap<TrackerCustomizationSettings, TrackerCustomizationSettings>();
        }
    }
}