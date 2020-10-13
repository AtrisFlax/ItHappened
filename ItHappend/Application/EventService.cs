using System;
using System.Reflection.Metadata.Ecma335;
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

        public void CreateEvent(Event newEvent)
        {
            _eventRepository.SaveEvent(newEvent);
        }

        public void EditEvent(Guid eventId, Guid eventCreatorId, Event newEvent)
        {
            var forEditingEvent = _eventRepository.LoadEvent(eventId);
            if (eventCreatorId != forEditingEvent.CreatorId)
            {
                throw new AuthenticationException();
            }
            if (newEvent.Id != eventId)
            {
                throw new Exception();
            }
            _eventRepository.SaveEvent(newEvent);
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