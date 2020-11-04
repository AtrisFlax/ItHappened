using System;
using System.Collections.Generic;
using ItHappened.Domain;

namespace ItHappened.Application.Services.TrackerService
{
    public interface ITrackerService
    {
        Guid CreateEventTracker(Guid creatorId, string trackerName, TrackerCustomizationSettings customizationSettings);
        EventTracker GetEventTracker(Guid actorId, Guid trackerId);
        IReadOnlyCollection<EventTracker> GetEventTrackers(Guid actorId);

        void EditEventTracker(Guid actorId,
            Guid trackerId,
            string name,
            TrackerCustomizationSettings customizationSettings);

        void DeleteEventTracker(Guid actorId, Guid trackerId);
    }
}