using System;
using System.Collections.Generic;
using ItHappened.Domain;
using LanguageExt;

namespace ItHappened.Application.Services.EventTrackerService
{
    public interface IEventTrackerService
    {
        Guid CreateTracker(
            Guid creatorId,
            string trackerName,
            bool hasPhoto,
            bool hasScale,
            string scaleMeasurementUnit,
            bool hasRating,
            bool hashGeoTag,
            bool hasComment);
        
        EventTrackerServiceStatusCodes DeleteTracker(Guid trackerId, Guid creatorId);

        IReadOnlyCollection<EventTracker> GetAllUserTrackers(Guid userId);
        EventTrackerServiceStatusCodes AddEventToTracker(Guid initiatorId, Guid trackerId, Event @event);
        EventTrackerServiceStatusCodes RemoveEventFromTracker(Guid initiatorId, Guid trackerId, Guid eventId);
        EventTrackerServiceStatusCodes EditEventInTracker(Guid initiatorId, Guid trackerId, Event newEvent);
        (IReadOnlyCollection<Event> collection, EventTrackerServiceStatusCodes statusCode) GetAllEventsFromTracker(Guid trackerId, Guid initiatorId);
        (IReadOnlyCollection<Event> collection, EventTrackerServiceStatusCodes statusCode) FilterEvents(
            Guid trackerId,
            Guid initiatorId,
            IReadOnlyList<IEventsFilter> filters);
    }
}