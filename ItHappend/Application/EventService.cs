using System;
using ItHappend.Domain;

namespace ItHappend
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
            return loadedEvent.CreatorUserId != eventCreatorId ? null : loadedEvent;
        }
        //CreationDate предлагаю не передавать, а создават в конструкторе создаваемой сущности
        //evaluation тоже не передавать, т.к. это опциональный параметр
        public Guid CreateEvent(Guid eventId, Guid creatorId, string name, DateTimeOffset creationDate,
            decimal evaluation)
        {
            var newEvent = new Event(eventId, creatorId, name, creationDate, evaluation);
            return _eventRepository.SaveEvent(newEvent);
        }
        //То же самое
        // в If условие должно быть !=
        public void EditEvent(Guid eventId, Guid eventCreatorId, string newName, DateTimeOffset eventHappensDate,
            decimal evaluation)
        {
            var forEditingEvent = _eventRepository.LoadEvent(eventId);
            if (eventCreatorId == forEditingEvent.CreatorUserId)
            {
                throw new Exception();
            }
            forEditingEvent.EditEvent(newName, eventHappensDate, evaluation);
            _eventRepository.SaveEvent(forEditingEvent);
        }
        // в If условие должно быть !=
        public void DeleteEvent(Guid eventId, Guid creatorId)
        {
            var forDeleteEvent = _eventRepository.LoadEvent(eventId);
            if (creatorId == forDeleteEvent.CreatorUserId)
            {
                throw new Exception();
            }
            _eventRepository.DeleteEvent(eventId);
        }
    }
}