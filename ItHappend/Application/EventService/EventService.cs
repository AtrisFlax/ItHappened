using System;
using ItHappend.Domain;
using Status = ItHappend.Application.EventServiceStatusCodes;

namespace ItHappend.Application
{
    class EventService : IEventService
    {
        private readonly IEventRepository _eventRepository;

        public EventService(IEventRepository eventRepository)
        {
            _eventRepository = eventRepository;
        }

        public (Event @event, Status operationStatus) TryGetEvent(Guid eventId, Guid eventCreatorId)
        {
            var loadedEvent = _eventRepository.TryLoadEvent(eventId);
            return loadedEvent.CreatorId != eventCreatorId ? (null, Status.WrongCreatorId) : (loadedEvent, Status.Ok);
        }

        public Status CreateEvent(Event newEvent)
        {
            _eventRepository.SaveEvent(newEvent);
            return Status.Ok;
        }

        public Status TryEditEvent(Guid eventId, Guid eventCreatorId, Event newEvent)
        {
            var forEditingEvent = _eventRepository.TryLoadEvent(eventId);
            if (eventCreatorId != forEditingEvent.CreatorId) return Status.WrongCreatorId;
            if (newEvent.Id != eventId) return Status.WrongEventId;
            _eventRepository.SaveEvent(newEvent);
            return Status.Ok;
        }

        public Status TryDeleteEvent(Guid eventId, Guid creatorId)
        {
            var forDeleteEvent = _eventRepository.TryLoadEvent(eventId);
            if (creatorId != forDeleteEvent.CreatorId) return Status.WrongCreatorId;
            _eventRepository.DeleteEvent(eventId);
            return Status.Ok;
        }
    }
}