using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using ItHappened.Domain;
using ItHappened.Infrastructure.Dto;
using LanguageExt.UnsafeValueAccess;

namespace ItHappened.Infrastructure.EFCoreRepositories
{
    // ReSharper disable once InconsistentNaming
    public class EFEventsRepository : IEventRepository
    {
        private readonly ItHappenedDbContext _context;
        private readonly IMapper _mapper;

        public EFEventsRepository(ItHappenedDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public void SaveEvent(Event newEvent)
        {
            var eventDto = _mapper.Map<EventDto>(newEvent);
            _context.Events.Add(eventDto);
        }

        public void AddRangeOfEvents(IEnumerable<Event> events)
        {
            _context.Events.AddRange(_mapper.Map<EventDto>(events));
        }

        public Event LoadEvent(Guid eventId)
        {
            var eventDto = _context.Events.Find(eventId);
            return _mapper.Map<Event>(eventDto);
        }

        public IReadOnlyCollection<Event> LoadAllTrackerEvents(Guid trackerId)
        {
            var eventsDto = _context.Events.Where(@event => @event.TrackerId == trackerId);
            return _mapper.Map<IQueryable<EventDto>, List<Event>>(eventsDto);
        }

        public void UpdateEvent(Event @event)
        {
            var odlEventDto = _context.Events.Find(@event.Id);
            odlEventDto.HappensDate = @event.HappensDate;
            odlEventDto.Comment = @event.CustomizationsParameters.Comment.ValueUnsafe().Text;
            odlEventDto.Rating = @event.CustomizationsParameters.Rating.ValueUnsafe();
            odlEventDto.Scale = @event.CustomizationsParameters.Scale.ValueUnsafe();
            odlEventDto.LatitudeGeo = @event.CustomizationsParameters.GeoTag.ValueUnsafe().GpsLat;
            odlEventDto.LongitudeGeo = @event.CustomizationsParameters.GeoTag.ValueUnsafe().GpsLng;
            odlEventDto.Photo = @event.CustomizationsParameters.Photo.ValueUnsafe().PhotoBytes;
        }

        public void DeleteEvent(Guid eventId)
        {
            var eventToDeleteDto = _context.Events.Find(eventId);
            _context.Events.Remove(eventToDeleteDto);
        }

        public bool IsContainEvent(Guid eventId)
        {
            return _context.Events.Any(o => o.Id == eventId);
        }
    }
}