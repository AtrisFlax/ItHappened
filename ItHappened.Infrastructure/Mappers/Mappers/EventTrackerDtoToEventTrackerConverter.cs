using AutoMapper;
using ItHappened.Domain;
using ItHappened.Infrastructure.Dto;
using LanguageExt;

namespace ItHappened.Infrastructure.Mappers
{  
    internal class EventTrackerDtoToEventTrackerConverter : ITypeConverter<EventTrackerDto, EventTracker>
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