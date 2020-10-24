using System.Collections.Generic;

namespace ItHappened.Domain
{
    public interface IEventsFilter
    {
        IEnumerable<Event> Filter(IEnumerable<Event> events);
    }
}