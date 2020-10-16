using System;
using System.Collections.Generic;
using System.Linq;

namespace ItHappened.Domain
{
    public class EventTracker
    {
        public Guid Id { get; }
        public string Name { get; }
        public IList<Event> Events { get; private set; }

        public Guid CreatorId { get; }

        public EventTracker(Guid id, string name, IList<Event> events, Guid creatorId)
        {
            Id = id;
            Name = name;
            Events = events;
            CreatorId = creatorId;
        }

        public void AddEvent(Event newEvent)
        {
            Events.Add(newEvent);
        }

        public bool RemoveEvent(Event eventToRemove)
        {
            return Events.Remove(eventToRemove);
        }

        public IReadOnlyCollection<Event> FilterEventsByTimeSpan(DateTimeOffset from, DateTimeOffset to)
        {
            var filteredEvents = Events.Where(eventItem =>
                eventItem.HappensDate.UtcDateTime >= from.UtcDateTime &&
                eventItem.HappensDate.UtcDateTime <= to.UtcDateTime).ToArray();
            return filteredEvents;
        }
    }
}