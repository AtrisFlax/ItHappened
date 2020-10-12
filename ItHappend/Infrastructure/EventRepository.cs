using System;
using System.Collections.Generic;
using ItHappend.Domain;
using ItHappend.Domain.Exceptions;

namespace ItHappend.Infrastructure
{
    public class EventRepository : IEventRepository
    {
        private readonly Dictionary<Guid, Event> _events = new Dictionary<Guid, Event>();
        public void AddEvent(Event newEvent)
        {
            _events.Add(newEvent.EventId, newEvent);
        }

        public Event GetEvent(Guid eventId)
        {
            if (!_events.ContainsKey(eventId))
            {
                throw new EventNotFoundException(eventId);
            }

            return _events[eventId];
        }

        public void DeleteEvent(Guid eventId)
        {
            _events.Remove(eventId);
        }
    }
}