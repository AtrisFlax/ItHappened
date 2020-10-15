using System;
using System.Collections.Generic;
using ItHappend.Domain;

namespace ItHappend.EventService
{
    public interface IEventTrackerService
    {
        Guid CreateTracker(Guid creatorId, string trackerName);
        void AddEventToTracker(Guid trackerId, Guid eventId, Guid initiatorId);
        void RemoveEventFromTracker(Guid trackerId, Guid eventId, Guid initiatorId);
        public void DeleteTracker(Guid trackerId, Guid initiatorId);

        IReadOnlyCollection<Event> FilterTrackerEventsByTimeSpan(Guid trackerId, Guid initiatorId,
            DateTimeOffset from, DateTimeOffset to);

    }
}