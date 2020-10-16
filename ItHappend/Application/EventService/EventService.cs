using System;
using ItHappend.Domain;
using ItHappend.Domain.EventCustomization;
using ItHappend.EventService;
using LanguageExt;

namespace ItHappend.Application
{
    public class EventService : IEventService
    {
        private readonly IEventRepository _eventRepository;

        public EventService(IEventRepository eventRepository)
        {
            _eventRepository = eventRepository;
        }

        public Option<Event> TryGetEvent(Guid eventId, Guid eventCreatorId)
        {
            var loadedEvent = _eventRepository.TryLoadEvent(eventId);
            return loadedEvent.CreatorId != eventCreatorId
                ? Option<Event>.None
                : Option<Event>.Some(loadedEvent);
        }

        public Guid CreateEvent(Guid id,
            Guid creatorId,
            DateTimeOffset happensDate,
            string title,
            Photo photo = null,
            double? scale = null,
            double? rating = null,
            GeoTag geoTag = null,
            string comment = null)
        {
            var builtEvent = BuildEvent(id, creatorId, happensDate, title, photo, @scale, rating, geoTag, comment);

            _eventRepository.SaveEvent(builtEvent);
            return builtEvent.Id;
        }

        public bool TryEditEvent(Guid eventId, Guid eventCreatorId, Event eventToReplace)
        {
            var eventToEdit = _eventRepository.TryLoadEvent(eventId);
            if (!ValidateCreatorId(eventCreatorId, eventToEdit)) return false;
            if (!EventForReplaceEventIdValidation(eventId, eventToReplace)) return false;
            _eventRepository.SaveEvent(eventToReplace);
            return true;
        }

        public bool TryDeleteEvent(Guid eventId, Guid creatorId)
        {
            var forDeleteEvent = _eventRepository.TryLoadEvent(eventId);
            if (creatorId != forDeleteEvent.CreatorId)
            {
                return false;
            }

            _eventRepository.DeleteEvent(eventId);
            return true;
        }

        private static bool EventForReplaceEventIdValidation(Guid eventId, Event newEventForReplace)
        {
            return newEventForReplace.Id == eventId;
        }

        private static bool ValidateCreatorId(Guid eventCreatorId, Event forEditingEvent)
        {
            return eventCreatorId == forEditingEvent.CreatorId;
        }

        private static Event BuildEvent(Guid id, Guid creatorId, DateTimeOffset happensDate, string title, Photo photo,
            double? scale, double? rating, GeoTag geoTag, string comment)
        {
            var builder = EventBuilder.Event(id,
                creatorId,
                happensDate,
                title);

            if (photo != null)
            {
                builder.WithPhoto(photo);
            }

            if (scale.HasValue)
            {
                builder.WithScale((double) scale);
            }

            if (rating.HasValue)
            {
                builder.WithRating((double) rating);
            }

            if (geoTag != null)
            {
                builder.WithGeoTag(geoTag);
            }

            if (comment != null)
            {
                builder.WithComment(comment);
            }

            return builder.Build();
        }
    }
}