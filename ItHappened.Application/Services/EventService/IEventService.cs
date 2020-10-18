using System;
using ItHappened.Domain;
using LanguageExt;

namespace ItHappened.Application.Services.EventService
{
    public interface IEventService
    {
        Option<Event> TryGetEvent(Guid eventId, Guid eventCreatorId);

        Guid CreateEvent(Guid id,
            Guid creatorId,
            DateTimeOffset happensDate,
            string title,
            Photo photo = null,
            double? scale = null,
            double? rating = null,
            GeoTag geoTag = null,
            string comment = null);

        bool TryEditEvent(Guid eventId, Guid eventCreatorId, Event eventToReplace);
        bool TryDeleteEvent(Guid eventId, Guid creatorId);
    }
}