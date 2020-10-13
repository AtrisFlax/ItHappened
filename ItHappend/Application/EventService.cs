using System;
using System.Security.Authentication;
using ItHappend.Domain;

namespace ItHappend.Application
{
    class EventService : IEventService
    {
        private readonly IEventRepository _eventRepository;

        public EventService(IEventRepository eventRepository)
        {
            _eventRepository = eventRepository;
        }

        public Event GetEvent(Guid eventId, Guid eventCreatorId)
        {
            var loadedEvent = _eventRepository.LoadEvent(eventId);
            return loadedEvent.CreatorId != eventCreatorId ? null : loadedEvent;
        }

        public void CreateEvent(Guid eventId, Guid creatorId, string name, DateTimeOffset creationDate,
            decimal evaluation)
        {
            var newEvent = new Event(eventId, creatorId, name, creationDate, evaluation);
            _eventRepository.SaveEvent(newEvent);
        }

        public void EditEvent(Guid eventId, Guid eventCreatorId, string newName, DateTimeOffset eventHappensDate,
            decimal evaluation)
        {
            var forEditingEvent = _eventRepository.LoadEvent(eventId);
            if (eventCreatorId != forEditingEvent.CreatorId)
            {
                throw new AuthenticationException();
            }
            forEditingEvent.EditEvent(newName, eventHappensDate, evaluation);
            _eventRepository.SaveEvent(forEditingEvent);
        }

        public void DeleteEvent(Guid eventId, Guid creatorId)
        {
            var forDeleteEvent = _eventRepository.LoadEvent(eventId);
            if (creatorId != forDeleteEvent.CreatorId)
            {
                throw new AuthenticationException();
            }
            _eventRepository.DeleteEvent(eventId);
        }
    }
}