using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using ItHappened.Domain;
using ItHappened.Infrastructure.Mappers;
using Microsoft.EntityFrameworkCore;

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
            var eventsDto = _context.Events.Where(@event => @event.TrackerId == trackerId).ToList();
            return _mapper.Map<Event[]>(eventsDto.ToList());
        }

        public void UpdateEvent(Event @event)
        {
            var eventDto = _mapper.Map<EventDto>(@event);
            _context.Events.Update(eventDto);
        }

        public void DeleteEvent(Guid eventId)
        {
            //Deleting without loading from the database
            var @event = _context.Events.First(eventDto => eventDto.Id == eventId);
            _context.Events.Remove(@event);
        }

        public bool IsContainEvent(Guid eventId)
        {
            return _context.Events.Any(o => o.Id == eventId);
        }
    }
}