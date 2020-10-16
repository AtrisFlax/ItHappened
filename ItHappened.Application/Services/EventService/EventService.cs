using System;
using ItHappened.Bll.Domain;
using ItHappened.Bll.Domain.Repositories;

namespace ItHappened.Application.Services.EventService
{
    public class EventService : IEventService
    {
        private readonly IEventRepository _eventRepository;

        public EventService(IEventRepository eventRepository)
        {
            _eventRepository = eventRepository;
        }

        public (Event @event, EventServiceStatusCodes operationStatus) TryGetEvent(Guid eventId, Guid eventCreatorId)
        {
            var loadedEvent = _eventRepository.TryLoadEvent(eventId);
            return loadedEvent.CreatorId != eventCreatorId ? (null, EventServiceStatusCodes.WrongCreatorId) : (loadedEvent, EventServiceStatusCodes.Ok);
        }

        public EventServiceStatusCodes CreateEvent(Event newEvent)
        {
            _eventRepository.SaveEvent(newEvent);
            return EventServiceStatusCodes.Ok;
        }

        public EventServiceStatusCodes TryEditEvent(Guid eventId, Guid eventCreatorId, Event newEvent)
        {
            var forEditingEvent = _eventRepository.TryLoadEvent(eventId);
            if (eventCreatorId != forEditingEvent.CreatorId) return EventServiceStatusCodes.WrongCreatorId;
            if (newEvent.Id != eventId) return EventServiceStatusCodes.WrongEventId;
            _eventRepository.SaveEvent(newEvent);
            return EventServiceStatusCodes.Ok;
        }

        public EventServiceStatusCodes TryDeleteEvent(Guid eventId, Guid creatorId)
        {
            var forDeleteEvent = _eventRepository.TryLoadEvent(eventId);
            if (creatorId != forDeleteEvent.CreatorId) return EventServiceStatusCodes.WrongCreatorId;
            _eventRepository.DeleteEvent(eventId);
            return EventServiceStatusCodes.Ok;
        }
    }
}