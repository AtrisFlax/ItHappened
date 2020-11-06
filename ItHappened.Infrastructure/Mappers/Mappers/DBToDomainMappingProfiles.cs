using AutoMapper;
using ItHappened.Domain;
using ItHappened.Infrastructure.Dto;
using LanguageExt;

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
                geoTag = Option<GeoTag>.Some(new GeoTag(source.LatitudeGeo.Value, source.LongitudeGeo.Value)); //TODO  Possible 'System.InvalidOperationException'
            }

            var photo = source.Photo.Length != 0 ? Option<Photo>.Some(new Photo(source.Photo)) : Option<Photo>.None;
            var eventCustomParameters = new EventCustomParameters(photo, scale, rating, geoTag, comment);
            
            return new Event(source.Id, source.CreatorId, source.TrackerId, source.HappensDate,
                eventCustomParameters);
        }
    }
}