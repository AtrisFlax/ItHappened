using System;
using System.Collections.Generic;
using ItHappened.Domain;
using ItHappened.Domain.Repositories;

namespace ItHappened.Infrastructure.Repositories
{
    public class EventRepository : IEventRepository
    {
        private readonly Dictionary<Guid, Event> _events = new Dictionary<Guid, Event>();
        public void SaveEvent(Event newEvent)
        {
            _events.Add(newEvent.Id, newEvent);
        }

        public Event TryLoadEvent(Guid eventId)
        {
            return _events[eventId];
        }

        public void DeleteEvent(Guid eventId)
        {
            _events.Remove(eventId);
        }
    }
}