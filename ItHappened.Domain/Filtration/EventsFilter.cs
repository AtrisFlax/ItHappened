using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Serilog;

namespace ItHappened.Domain
{
    public static class EventsFilter
    {
        public static IReadOnlyList<Event> Filter(IReadOnlyCollection<Event> events,
            IReadOnlyCollection<IEventsFilter> filters)
        {
            return filters.Aggregate(events, (current, filter) => filter.Filter(current)).ToList();
        }
    }
}