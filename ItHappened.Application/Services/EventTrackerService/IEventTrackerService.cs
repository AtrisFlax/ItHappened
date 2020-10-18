using System;
using System.Collections.Generic;
using ItHappened.Domain;

namespace ItHappened.Application.Services.EventTrackerService
{
    public interface IEventTrackerService
    {
        Guid CreateTracker(Guid creatorId, string trackerName);
        bool TryAddEventToTracker(Guid trackerId, Guid eventId, Guid initiatorId);
        void RemoveEventFromTracker(Guid trackerId, Guid eventId, Guid initiatorId);
        void DeleteTracker(Guid trackerId, Guid initiatorId);
        
        IReadOnlyCollection<EventTracker> GetAllTrackers(Guid userId);
        
        
        IReadOnlyCollection<Event> FilterTrackerEventsByTimeSpan(Guid trackerId, Guid initiatorId, 
            DateTimeOffset from, DateTimeOffset to);
    }
}