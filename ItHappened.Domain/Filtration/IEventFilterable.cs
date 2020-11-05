using System;
using System.Collections.Generic;

namespace ItHappened.Domain
{
    public interface IEventFilterable
    {
        IReadOnlyCollection<Event> GetAllFilteredEvents(Guid actorId, Guid trackerId, EventFilterData eventFilterData);
    }
}