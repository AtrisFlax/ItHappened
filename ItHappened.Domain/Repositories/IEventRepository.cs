using System;
using System.Collections.Generic;

namespace ItHappened.Domain
{
    public interface IEventRepository
    {
        void AddEvent(Event newEvent);
        void AddRangeOfEvents(IEnumerable<Event> events);
        Event LoadEvent(Guid eventId);
        IReadOnlyList<Event> LoadAllTrackerEvents(Guid trackerId);
        void DeleteEvent(Guid eventId);
    }
}