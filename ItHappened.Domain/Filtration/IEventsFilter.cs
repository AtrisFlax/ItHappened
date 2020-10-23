using System.Collections.Generic;

namespace ItHappened.Domain
{
    public interface IEventsFilter
    {
        IReadOnlyCollection<Event> Filter(IReadOnlyCollection<Event> events);
    }
}