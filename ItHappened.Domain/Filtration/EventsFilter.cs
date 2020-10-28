using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Serilog;

namespace ItHappened.Domain
{
    public static class EventsFilter
    {
        public static IReadOnlyCollection<Event> Filter(IEnumerable<Event> events,
            IEnumerable<IEventsFilter> filters)
        {
            IEnumerable<Event> result = events;
            foreach (var filter in filters)
            {
                result = filter.Filter(result);
            }
            return result.ToList();
        }
    }
} 