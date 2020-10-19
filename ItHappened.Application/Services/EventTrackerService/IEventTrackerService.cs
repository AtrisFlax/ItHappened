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
            bool hasPhoto = false,
            bool hasScale = false,
            string scaleMeasurementUnit = "",
            bool hasRating = false,
            bool hashGeoTag = false,
            bool hasComment = false);
        
        EventTrackerServiceStatusCodes DeleteTracker(Guid trackerId, Guid trackerCreatorId);

        IEnumerable<EventTracker> GetAllUserTrackers(Guid trackerCreatorId);
        Option<EventTracker> GetTracker(Guid trackerCreatorId, Guid trackerId);
        bool AddEventToTracker(Guid trackerCreatorId, Guid trackerId, Event @event);
        bool RemoveEventFromTracker(Guid trackerCreatorId, Guid trackerId, Guid eventId);
        bool EditEventInTracker(Guid trackerCreatorId, Guid trackerId, Guid eventId, Event newEvent);
        Option<IList<Event>> GetAllEventsFromTracker(Guid trackerId, Guid trackerCreatorId);

        // Option<IReadOnlyCollection<Event>> GetEventsFiltratedByTime(Guid trackerCreatorId, Guid trackerId, 
        //     DateTimeOffset from, DateTimeOffset to);
    }
}