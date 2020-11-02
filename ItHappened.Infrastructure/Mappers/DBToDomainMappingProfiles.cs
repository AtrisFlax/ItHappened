using AutoMapper;
using ItHappened.Domain;
using LanguageExt;
using Microsoft.AspNetCore.Routing.Constraints;

namespace ItHappened.Infrastructure.Mappers
{
    public class DbToDomainMappingProfiles : Profile
    {
        public DbToDomainMappingProfiles()
        {
            CreateMap<UserDto, User>();
            CreateMap<EventTrackerDto, EventTracker>().ConvertUsing<EventTrackerDtoToEventTrackerConverter>();

        }
    }

    public class EventTrackerDtoToEventTrackerConverter : ITypeConverter<EventTrackerDto, EventTracker>
    {
        public EventTracker Convert(EventTrackerDto source, EventTracker destination, ResolutionContext context)
        {
            var scaleMeasurementUnit = source.ScaleMeasurementUnit.IsNull() ? 
                Option<string>.None : Option<string>.Some(source.ScaleMeasurementUnit);
            
            var tracker = new EventTracker(source.Id, source.CreatorId, source.Name,
                new TrackerCustomizationSettings(source.IsPhotoRequired, source.IsScaleRequired, scaleMeasurementUnit,
                    source.IsRatingRequired, source.IsGeotagRequired, source.IsCommentRequired,
                    source.IsCustomizationRequired));
            return tracker;
        }
    }
}