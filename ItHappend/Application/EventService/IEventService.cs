using System;
using ItHappend.Application;
using ItHappend.Domain;
using Optional;

namespace ItHappend
{
    public interface IEventService
    {
        Option<Event, EventServiceStatusCodes> TryGetEvent(Guid eventId, Guid eventCreatorId);

        Guid CreateEvent(Guid id,
            Guid creatorId,
            DateTimeOffset happensDate,
            string title,
            Photo photo = null,
            double? scale = null,
            double? rating = null,
            GeoTag geoTag = null,
            string comment = null);

        bool TryEditEvent(Guid eventId, Guid eventCreatorId, Event eventForReplace);
        bool TryDeleteEvent(Guid eventId, Guid creatorId);
    }
}