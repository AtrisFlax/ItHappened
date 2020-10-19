using System;
using System.Collections.Generic;
using ItHappened.Domain;

namespace ItHappened.Infrastructure.Repositories
{
    public class EventRepository : IEventRepository
    {
        private readonly Dictionary<Guid, Event> _events = new Dictionary<Guid, Event>();

        public void AddEvent(Event newEvent)
        {
            _events.Add(newEvent.Id, newEvent);
        }

        public Event LoadEvent(Guid eventId)
        {
            return _events[eventId];
        }

        public void DeleteEvent(Guid eventId)
        {
            _events.Remove(eventId);
        }
    }
}