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
            bool hasRating = false,
            bool hashGeoTag = false,
            bool hasComment = false);

        bool DeleteTracker(Guid trackerId, Guid trackerCreatorId);
        bool AddEventToTracker(Guid trackerId, Guid trackerCreatorId, Event @event);
        bool RemoveEventFromTracker(Guid trackerId, Guid eventId, Guid trackerCreatorId);
        bool EditEventTracker(Guid trackerId, Guid trackerCreatorId, Event @event);

        Option<IReadOnlyCollection<Event>> FilterByTime(Guid trackerId, Guid trackerCreatorId,
            DateTimeOffset from, DateTimeOffset to);
    }
}