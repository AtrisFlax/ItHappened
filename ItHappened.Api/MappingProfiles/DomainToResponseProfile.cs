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
            CreateMap<Event, EventResponse>()
                .ForMember(dest => dest.Comment, opt =>
                    opt.MapFrom(src => src.CustomizationsParameters.Comment.Match((c) => c.Text, () => null)))
                // .ForMember(dest => dest.Photo, opt =>
                //     opt.MapFrom(src => src.CustomizationsParameters.Photo.ValueUnsafe())) #TO DO ssue 148
                .ForMember(dest => dest.Rating, opt =>
                    opt.MapFrom(src => src.CustomizationsParameters.Rating.ValueUnsafe()))
                .ForMember(dest => dest.Scale, opt =>
                    opt.MapFrom(src => src.CustomizationsParameters.Scale.ValueUnsafe()))
                .ForMember(dest => dest.GeoTag, opt =>
                    opt.MapFrom(src => src.CustomizationsParameters.GeoTag.ValueUnsafe()));
            
            
            CreateMap<TrackerCustomizationSettings, CustomizationSettingsResponse>()
                .ForMember(dest => dest.ScaleMeasurementUnit, opt => 
                    opt.MapFrom(src => src.ScaleMeasurementUnit.ValueUnsafe()));
        }
    }
}