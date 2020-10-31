using AutoMapper;
using ItHappened.Domain;
using LanguageExt.UnsafeValueAccess;

namespace ItHappened.Infrastructure.Mappers
{
    public class DomainToDbMappingProfiles : Profile
    {
        public DomainToDbMappingProfiles()
        {
            CreateMap<User, UserDto>();

            CreateMap<Event, EventDto>()
                .ForMember(
                    dest => dest.Comment,
                    opt => opt.MapFrom(
                        src => src.CustomizationsParameters.Comment.Match(c => c.Text, () => null)
                    )
                )
                .ForMember(dest => dest.Photo, opt =>
                    opt.MapFrom(src => src.CustomizationsParameters.Photo.ValueUnsafe()))
                .ForMember(dest => dest.Rating, opt =>
                    opt.MapFrom(src => src.CustomizationsParameters.Rating.ValueUnsafe()))
                .ForMember(dest => dest.Scale, opt =>
                    opt.MapFrom(src => src.CustomizationsParameters.Scale.ValueUnsafe()))
                .ForMember(dest => dest.LatitudeGeo, opt =>
                    opt.MapFrom(src => src.CustomizationsParameters.GeoTag.ValueUnsafe().GpsLat))
                .ForMember(dest => dest.LongitudeGeo, opt =>
                    opt.MapFrom(src => src.CustomizationsParameters.GeoTag.ValueUnsafe().GpsLng));


            CreateMap<EventTracker, EventTrackerDto>()
            /*.ForMember(
                dest => dest.Id,
                opt => opt.MapFrom(src => src.Id)
                )
            .ForMember(
                dest => dest.CreatorId,
                opt => opt.MapFrom(src => src.CreatorId)
            )
            .ForMember(
                dest => dest.Name,
                opt => opt.MapFrom(src => src.Name)
            )
            .ForMember(
                dest => dest.Id,
                opt => opt.MapFrom(src => src.Id)
            )
            .ForMember(
                dest => dest.IsPhotoRequired,
                opt => opt.MapFrom(src => src.CustomizationSettings.IsPhotoRequired)
            )
            .ForMember(
                dest => dest.IsScaleRequired,
                opt => opt.MapFrom(src => src.CustomizationSettings.IsScaleRequired)
            )
            .ForMember(
                dest => dest.IsRatingRequired,
                opt => opt.MapFrom(src => src.CustomizationSettings.IsRatingRequired)
            )
            .ForMember(
                dest => dest.IsGeotagRequired,
                opt => opt.MapFrom(src => src.CustomizationSettings.IsGeotagRequired)
            )
            .ForMember(
                dest => dest.IsCommentRequired,
                opt => opt.MapFrom(src => src.CustomizationSettings.IsCommentRequired)
            )
            .ForMember(
                dest => dest.IsCustomizationRequired,
                opt => opt.MapFrom(src => src.CustomizationSettings.IsCustomizationRequired)
            )*/;



        }
    }
}