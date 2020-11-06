using System;
using System.Collections.Generic;

namespace ItHappened.Domain
{
    public interface IEventFiltrationRepository
    {
        IReadOnlyCollection<Event> GetAllFilteredEvents(Guid actorId, Guid trackerId,
            EventFilterData eventFilterData);
    }
}