using System;
using System.Collections.Generic;
using ItHappened.Domain;

namespace ItHappened.Application.Services.EventService
{
    public interface IEventService
    {
        Guid CreateEvent(Guid actorId, Guid trackerId, DateTimeOffset eventHappensDate,
            EventCustomParameters customParameters);

        void AddRangeEvent(Guid actorId, Guid trackerId, IEnumerable<EventsInfoRange> eventsInfoRange);

        Event GetEvent(Guid actorId, Guid eventId);
        IReadOnlyCollection<Event> GetAllEvents(Guid actorId, Guid trackerId);

        Event EditEvent(Guid actorId,
            Guid eventId,
            DateTimeOffset timeStamp,
            EventCustomParameters customParameters);

        Event DeleteEvent(Guid actorId, Guid eventId);

        public IReadOnlyCollection<Event> GetAllFilteredEvents(Guid actorId, Guid trackerId, IEnumerable<IEventsFilter> eventsFilters);
    }
}