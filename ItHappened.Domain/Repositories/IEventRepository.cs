using System;
using System.Collections.Generic;

namespace ItHappened.Domain
{
    public interface IEventRepository
    {
        void SaveEvent(Event newEvent);
        void AddRangeOfEvents(IEnumerable<Event> events);
        Event LoadEvent(Guid eventId);
        IReadOnlyCollection<Event> LoadAllTrackerEvents(Guid trackerId);
        void UpdateEvent(Event @event);
        void DeleteEvent(Guid eventId);
    }
}