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
            //CreateMap<EventCustomParametersRequest, EventCustomParameters>();
            CreateMap<PhotoRequest, Photo>();
            CreateMap<GeoTagRequest, GeoTag>();

            CreateMap<EventCustomParametersRequest, EventCustomParameters>()
                .ForMember(dest => dest.Comment, opt =>
                    opt.MapFrom(src => Option<string>.Some(src.Comment)))
                .ForMember(dest => dest.Photo, opt =>
                    opt.MapFrom(src => Option<Photo>.Some(new Photo(src.Photo.PhotoBytes))))
                .ForMember(dest => dest.Rating, opt =>
                    opt.MapFrom(src => Option<double>.Some(src.Rating)))
                .ForMember(dest => dest.Scale, opt =>
                    opt.MapFrom(src => Option<double>.Some(src.Scale)))
                .ForMember(dest => dest.GeoTag, opt =>
                    opt.MapFrom(src => Option<GeoTag>.Some(new GeoTag(src.GeoTag.GpsLat, src.GeoTag.GpsLng))));
        }
    }
}