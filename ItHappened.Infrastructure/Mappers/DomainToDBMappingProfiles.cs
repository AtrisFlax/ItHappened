using AutoMapper;
using ItHappened.Domain;
using ItHappened.Domain.Statistics;
using ItHappened.Infrastructure.Dto;
using LanguageExt.UnsafeValueAccess;

namespace ItHappened.Infrastructure.Mappers
{
    public class DomainToDbMappingProfiles : Profile
    {
        public DomainToDbMappingProfiles()
        {
            //user
            CreateMap<User, UserDto>();
            //event
            CreateMap<Event, EventDto>()
                .ForMember(dest => dest.Comment, opt => opt.MapFrom(
                    src => src.CustomizationsParameters.Comment.ValueUnsafe().Text))
                .ForMember(dest => dest.Photo, opt =>
                    opt.MapFrom(src => src.CustomizationsParameters.Photo.MatchUnsafe(value => value.PhotoBytes, () => null)))
                .ForMember(dest => dest.Rating, opt =>
                    opt.MapFrom(src => (double?)src.CustomizationsParameters.Rating.IfNone(null) ))
                .ForMember(dest => dest.Scale, opt =>
                    opt.MapFrom(src =>  (double?)src.CustomizationsParameters.Scale.IfNone(null)))
                .ForMember(dest => dest.LatitudeGeo, opt =>
                    opt.MapFrom(src => src.CustomizationsParameters.GeoTag.ValueUnsafe().GpsLat))
                .ForMember(dest => dest.LongitudeGeo, opt =>
                    opt.MapFrom(src => src.CustomizationsParameters.GeoTag.ValueUnsafe().GpsLng));

            //tracker
            CreateMap<EventTracker, EventTrackerDto>()
                .ForMember(dest => dest.ScaleMeasurementUnit, opt =>
                    opt.MapFrom(src => src.CustomizationSettings.ScaleMeasurementUnit.ValueUnsafe()))
                .ForMember(dest => dest.IsCommentRequired, opt => opt.MapFrom(
                    src => src.CustomizationSettings.IsCommentRequired))
                .ForMember(dest => dest.IsGeotagRequired, opt => opt.MapFrom(
                    src => src.CustomizationSettings.IsGeotagRequired))
                .ForMember(dest => dest.IsPhotoRequired, opt => opt.MapFrom(
                    src => src.CustomizationSettings.IsPhotoRequired))
                .ForMember(dest => dest.IsRatingRequired, opt => opt.MapFrom(
                    src => src.CustomizationSettings.IsRatingRequired))
                .ForMember(dest => dest.IsScaleRequired, opt => opt.MapFrom(
                    src => src.CustomizationSettings.IsScaleRequired))
                .ForMember(dest => dest.IsCustomizationRequired, opt => opt.MapFrom(
                    src => src.CustomizationSettings.IsCustomizationRequired));

            //facts
            CreateMap<AverageRatingTrackerFact, AverageRatingTrackerFactDto>();
            CreateMap<AverageScaleTrackerFact, AverageScaleTrackerFactDto>();
            CreateMap<BestRatingEventFact, BestRatingEventFactDto>()
                .ForMember(dest => dest.BestEventComment,
                    opt => opt.MapFrom(src => src.BestEventComment.ValueUnsafe().Text));
            CreateMap<EventsCountTrackersFact, EventsCountTrackersFactDto>();
            CreateMap<LongestBreakTrackerFact, LongestBreakTrackerFactDto>();
            CreateMap<MostEventfulDayTrackersFact, MostEventfulDayTrackersFactDto>();
            CreateMap<MostEventfulWeekTrackersFact, MostEventfulWeekTrackersFactDto>();
            CreateMap<MostFrequentEventTrackersFact, MostFrequentEventTrackersFactDto>();
            CreateMap<OccursOnCertainDaysOfTheWeekTrackerFact, OccursOnCertainDaysOfTheWeekTrackerFactDto>();
            CreateMap<SingleTrackerEventsCountFact, SingleTrackerEventsCountFactDto>();
            CreateMap<SpecificDayTimeFact, SpecificDayTimeFactDto>();
            CreateMap<SumScaleTrackerFact, SumScaleTrackerFactDto>();
            CreateMap<WorstRatingEventFact, WorstRatingEventFactDto>()
                .ForMember(dest => dest.WorstEventComment,
                    opt => opt.MapFrom(src => src.WorstEventComment.ValueUnsafe().Text));
            ;
        }
    }
}