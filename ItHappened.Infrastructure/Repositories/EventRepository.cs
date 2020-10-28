using System;
using System.Collections.Generic;
using System.Linq;
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

        public void AddRangeOfEvents(IEnumerable<Event> events)
        {
            foreach (var @event in events)
            {
                AddEvent(@event);
            }
        }

        public Event LoadEvent(Guid eventId)
        {
            return _events[eventId];
        }

        public IReadOnlyCollection<Event> LoadAllTrackerEvents(Guid trackerId)
        {
            return _events.Values.Where(@event => @event.TrackerId == trackerId).ToList();
        }

        public void UpdateEvent(Event @event)
        {
            _events[@event.Id] = @event;
        }

        public void DeleteEvent(Guid eventId)
        {
            _events.Remove(eventId);
        }
    }
}