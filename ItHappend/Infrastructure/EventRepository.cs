using System;
using System.Collections.Generic;
using ItHappend.Domain;

namespace ItHappend.Infrastructure
{
    public class EventRepository : IEventRepository
    {
        private readonly Dictionary<Guid, Event> _events = new Dictionary<Guid, Event>();
        public void TrySaveEvent(Event newEvent)
        {
            _events.Add(newEvent.Id, newEvent);
        }

        public Event TryLoadEvent(Guid eventId)
        {
            return _events[eventId];
        }

        public void TryDeleteEvent(Guid eventId)
        {
            _events.Remove(eventId);
        }
    }
}