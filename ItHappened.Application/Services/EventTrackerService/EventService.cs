using System;
using System.Collections.Generic;
using ItHappened.Domain;

namespace ItHappened.Application.Services.EventTrackerService
{
    public class EventService : IEventService
    {
        public EventService(IEventRepository eventRepository, IEventTrackerRepository trackerRepository)
        {
            _eventRepository = eventRepository;
            _trackerRepository = trackerRepository;
        }
        
        public Event AddEvent(Guid actorId, Guid trackerId, DateTimeOffset timeStamp, EventCustomParameters customParameters)
        {
            var newEvent = new Event(Guid.NewGuid(), actorId, trackerId, timeStamp, customParameters);
            _eventRepository.AddEvent(newEvent);
            return newEvent;
        }
        
        public Event GetEvent(Guid actorId, Guid eventId)
        {
            var @event = _eventRepository.LoadEvent(eventId);
            if (actorId != @event.CreatorId)
                throw new Exception();
            return @event;
        }
        
        public IReadOnlyCollection<Event> GetAllEvents(Guid actorId, Guid trackerId)
        {
            var tracker = _trackerRepository.LoadTracker(trackerId);
            if (actorId != tracker.CreatorId)
                throw new Exception();
            var events = _eventRepository.LoadAllTrackerEvents(trackerId);
            return events;
        }
        
        public Event EditEvent(Guid actorId,
            Guid eventId,
            DateTimeOffset timeStamp,
            EventCustomParameters customParameters)
        {
            var tracker = _trackerRepository.LoadTracker(eventId);
            if (actorId != tracker.CreatorId)
                throw new Exception();

            var updatedEvent = new Event(Guid.NewGuid(), tracker.Id, tracker.CreatorId, timeStamp, customParameters);
            _eventRepository.UpdateEvent(updatedEvent);
            return updatedEvent;
        }

        public Event DeleteEvent(Guid actorId, Guid eventId)
        {
            var @event = _eventRepository.LoadEvent(eventId);
            if (actorId != @event.CreatorId)
                throw new Exception();

            _eventRepository.DeleteEvent(eventId);
            return @event;
        }
        
        
        private readonly IEventTrackerRepository _trackerRepository;
        private readonly IEventRepository _eventRepository;
    }
}