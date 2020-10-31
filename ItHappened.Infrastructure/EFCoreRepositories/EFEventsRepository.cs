using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using AutoMapper.Mappers;
using ItHappened.Domain;
using LanguageExt;

namespace ItHappened.Infrastructure.EFCoreRepositories
{
    public class EFEventsRepository : IEventRepository
    {
        private readonly ItHappenedDbContext _context;
        private IMapper _mapper;
        
        public EFEventsRepository(ItHappenedDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public void SaveEvent(Event newEvent)
        {
            var eventDto = _mapper.Map<EventDto>(newEvent);
            _context.Add(eventDto);
            _context.SaveChanges();
        }

        public void AddRangeOfEvents(IEnumerable<Event> events)
        {
            _context.AddRange(events);
            _context.SaveChanges();
        }

        public Event LoadEvent(Guid eventId)
        {
            var eventDbo = _context.Events.Find(eventId);
            return _mapper.Map<Event>(eventDbo);
        }

        public IReadOnlyCollection<Event> LoadAllTrackerEvents(Guid trackerId)
        {
            throw new NotImplementedException();
            //return _context.Events.Where(@event => @event.TrackerId == trackerId).ToList();
        }

        public void UpdateEvent(Event @event)
        {//TODO протестить
            _context.Update(@event);
            _context.SaveChanges();
        }

        public void DeleteEvent(Guid eventId)
        {
            _context.Remove(eventId);
            _context.SaveChanges();
        }

        public bool IsContainEvent(Guid eventId)
        {
            throw new NotImplementedException();
            //var @event = _context.Events.Find(eventId);
            //return !@event.IsNull();
        }
    }
}