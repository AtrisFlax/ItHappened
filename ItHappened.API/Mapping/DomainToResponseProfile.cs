using System;
using AutoMapper;
using ItHappened.Api.Models.Responses;
using ItHappened.Domain;
using LanguageExt.UnsafeValueAccess;

namespace ItHappened.Api.Mapping
{
    public class DomainToResponseProfile : Profile
    {
        public DomainToResponseProfile()
        {
            //To Tracker
            CreateMap<EventTracker, TrackerGetResponse>();

            //To Tracker
            CreateMap<Guid, TrackerPostResponse>()
                .ForMember(dest => dest.Id,
                    opt => opt.MapFrom(src => src));

            //To Event
            CreateMap<Event, EventGetResponse>()
                .ForMember(dest => dest.Comment,
                    opt => opt.MapFrom(
                        src => src.CustomizationsParameters.Comment.ValueUnsafe().Text))
                .ForMember(dest => dest.Photo, opt =>
                    opt.MapFrom(src => src.CustomizationsParameters.Photo.IsSome? src.CustomizationsParameters.Photo.ValueUnsafe() :  null ))
                .ForMember(dest => dest.Rating, opt =>
                    opt.MapFrom(src => (double?) src.CustomizationsParameters.Rating.IfNone(null)))
                .ForMember(dest => dest.Scale, opt =>
                    opt.MapFrom(src => (double?) src.CustomizationsParameters.Scale.IfNone(null)))
                .ForMember(dest => dest.GeoTag, opt =>
                    opt.MapFrom(src => src.CustomizationsParameters.GeoTag.ValueUnsafe()));

            //To Event
            CreateMap<Guid, EventPostResponse>()
                .ForMember(dest => dest.Id,
                    opt => opt.MapFrom(src => src));

            //To Settings
            CreateMap<TrackerCustomizationSettings, CustomizationSettingsResponse>()
                .ForMember(dest => dest.ScaleMeasurementUnit, opt =>
                    opt.MapFrom(src => src.ScaleMeasurementUnit.ValueUnsafe()))
                .ForMember(dest => dest.IsCustomizationRequired, opt =>
                    opt.MapFrom(src => src.IsCustomizationRequired));
        }
    }
}