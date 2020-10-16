using System;
using ItHappend.Domain;
using ItHappend.Domain.EventCustomization;
using LanguageExt;

namespace ItHappend.EventService
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