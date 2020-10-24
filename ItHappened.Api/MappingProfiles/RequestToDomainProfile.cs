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
            //CreateMap<PhotoRequest, Photo>();
            //CreateMap<GeoTagRequest, GeoTag>();

            CreateMap<EventRequest, EventCustomParameters>()
                .ForMember(dest => dest.Comment, opt =>
                    opt.MapFrom(src => Option<Comment>.Some(new Comment(src.Comment))))
                /*.ForMember(dest => dest.Photo, opt =>
                    opt.MapFrom(src => Option<Photo>.Some(new Photo(src.Photo.PhotoBytes))))
                */
                .ForMember(dest => dest.Rating, opt =>
                    opt.MapFrom(src => Option<double>.Some(src.Rating)))
                .ForMember(dest => dest.Scale, opt =>
                    opt.MapFrom(src => Option<double>.Some(src.Scale)))
                .ForMember(dest => dest.GeoTag, opt =>
                    opt.MapFrom(src => Option<GeoTag>.Some(new GeoTag(src.GeoTag.GpsLat, src.GeoTag.GpsLng))));

            CreateMap<EventFilterRequest, DateTimeFilter>()
                .ForMember(s => s, opt => opt.MapFrom((src, s) => { DateTimeFilter dateTimeFilter = null; return dateTimeFilter; }));
                //.ConstructUsing(s => new DateTimeFilter("FilterOnDemand", s.FromDateTime.Value, s.ToDateTime.Value))
                //.ForMember(dest => dest.To, opt => opt.MapFrom(src => src.ToDateTime))
                //.ForMember(dest => dest.From, opt => opt.MapFrom(src => src.FromDateTime));
        }

        public class EventCustomParametersPlay
        {
            public EventCustomParametersPlay(Option<double> scale,
                Option<double> rating,
                Option<Comment> comment)
            {
                Scale = scale;
                Rating = rating;
                Comment = comment;
            }

            public Option<double> Scale { get; }
            public Option<double> Rating { get; }
            public Option<Comment> Comment { get; }
        }
    }
}