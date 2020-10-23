using AutoMapper;
using ItHappened.Api.Models.Responses;
using ItHappened.Domain;
using LanguageExt.UnsafeValueAccess;

namespace ItHappened.Api.MappingProfiles
{
    public class DomainToResponseProfile : Profile
    {
        public DomainToResponseProfile()
        {
            CreateMap<EventTracker, TrackerResponse>();
            CreateMap<Event, EventResponse>();
            CreateMap<EventCustomParameters, EventCustomParametersResponse>();
            CreateMap<TrackerCustomizationSettings, CustomizationSettingsResponse>()
                .ForMember(dest => dest.ScaleMeasurementUnit, opt => 
                    opt.MapFrom(src => src.ScaleMeasurementUnit.ValueUnsafe()));
        }
    }
}