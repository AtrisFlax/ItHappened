using System;
using System.Collections.Generic;
using ItHappened.Domain;

namespace ItHappened.Application.Services.TrackerService
{
    public interface ITrackerService
    {
        EventTracker CreateEventTracker(Guid creatorId, string name, TrackerCustomizationSettings customizationSettings);
        EventTracker GetEventTracker(Guid actorId, Guid trackerId);
        IReadOnlyCollection<EventTracker> GetEventTrackers(Guid actorId);

        EventTracker EditEventTracker(Guid actorId,
            Guid trackerId,
            string name,
            TrackerCustomizationSettings customizationSettings);

        EventTracker DeleteEventTracker(Guid actorId, Guid trackerId);
    }
}