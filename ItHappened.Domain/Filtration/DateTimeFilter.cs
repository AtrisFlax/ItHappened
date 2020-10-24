using System;
using System.Collections.Generic;
using System.Linq;

namespace ItHappened.Domain
{
    public class DateTimeFilter : IEventsFilter
    {
        public string Name { get; }
        public DateTimeOffset From { get; }
        public DateTimeOffset To { get; }

        public DateTimeFilter(string name, DateTimeOffset @from, DateTimeOffset to)
        {
            Name = name;
            From = @from;
            To = to;
        }

        public IEnumerable<Event> Filter(IReadOnlyCollection<Event> events)
        {
            return events
                .Where(eventItem =>
                    eventItem.HappensDate.UtcDateTime >= From.UtcDateTime &&
                    eventItem.HappensDate.UtcDateTime <= To.UtcDateTime).ToList();
        }
    }
}