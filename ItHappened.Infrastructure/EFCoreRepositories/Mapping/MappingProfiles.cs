using AutoMapper;
using ItHappened.Domain;
using LanguageExt.UnsafeValueAccess;

namespace ItHappened.Infrastructure.EFCoreRepositories.Mapping
{
    public class MappingProfiles : Profile
    {
        public MappingProfiles()
        {
            CreateMap<Event, EventDto>()
                .ForMember(
                    dest => dest.Comment,
                    opt => opt.MapFrom(
                        src => src.CustomizationsParameters.Comment.ValueUnsafe().Text))
                .ForMember(dest => dest.Rating, opt =>
                    opt.MapFrom(src => src.CustomizationsParameters.Rating.ValueUnsafe()))
                .ForMember(dest => dest.Scale, opt =>
                    opt.MapFrom(src => src.CustomizationsParameters.Scale.ValueUnsafe()))
                .ForMember(dest => dest.GpsLat, opt =>
                opt.MapFrom(src => src.CustomizationsParameters.GeoTag.ValueUnsafe().GpsLat))
                .ForMember(dest => dest.GpsLng, opt =>
                    opt.MapFrom(src => src.CustomizationsParameters.GeoTag.ValueUnsafe().GpsLng));   
            // .ForMember(dest => dest.Rating, opt =>
                //     opt.MapFrom(src => src.CustomizationsParameters.Rating.ValueUnsafe()))
                // .ForMember(dest => dest.Scale, opt =>
                //     opt.MapFrom(src => src.CustomizationsParameters.Scale.ValueUnsafe()))
                
        }
    }
}