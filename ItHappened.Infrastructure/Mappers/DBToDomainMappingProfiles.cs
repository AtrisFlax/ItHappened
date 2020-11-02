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
            CreateMap<EventDto, Event>().ConvertUsing<EventDtoToEventConverter>();
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
    
    public class EventDtoToEventConverter : ITypeConverter<EventDto, Event>
    {
        public Event Convert(EventDto source, Event destination, ResolutionContext context)
        {
            var scale = source.Scale ?? Option<double>.None;  
            var rating = source.Rating ?? Option<double>.None;
            var comment = source.Comment.IsNull()
                ? Option<Comment>.None
                : Option<Comment>.Some(new Comment(source.Comment));
            
            Option<GeoTag> geoTag;
            if (source.LatitudeGeo.IsNull() || source.LongitudeGeo.IsNull())
            {
                geoTag = Option<GeoTag>.None;
            }
            else
            {
                geoTag = Option<GeoTag>.Some(new GeoTag(source.LatitudeGeo.Value, source.LongitudeGeo.Value));
            }

            var photo = source.Photo.IsNull() ? Option<Photo>.None : Option<Photo>.Some(new Photo(source.Photo));
            var eventCustomParameters = new EventCustomParameters(photo, scale, rating, geoTag, comment);
            
            return new Event(source.Id, source.CreatorId, source.TrackerId, source.HappensDate,
                eventCustomParameters);
        }
    }
}