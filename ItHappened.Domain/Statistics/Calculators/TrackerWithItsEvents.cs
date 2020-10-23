using System.Collections.Generic;

namespace ItHappened.Domain.Statistics
{
    public class TrackerWithItsEvents
    {
        public EventTracker Tracker { get; }
        public IReadOnlyCollection<Event> Events { get; }

        public TrackerWithItsEvents(EventTracker tracker, IReadOnlyCollection<Event> events)
        {
            Tracker = tracker;
            Events = events;
        }
    }
}