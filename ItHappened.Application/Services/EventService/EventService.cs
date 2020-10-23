using System;
using System.Collections.Generic;
using ItHappened.Domain;

namespace ItHappened.Application.Services.EventService
{
    public class EventService : IEventService
    {
        private readonly ITrackerRepository _trackerRepository;
        private readonly IEventRepository _eventRepository;
        
        public EventService(IEventRepository eventRepository, ITrackerRepository trackerRepository)
        {
            _eventRepository = eventRepository;
            _trackerRepository = trackerRepository;
        }
        
        public Event AddEvent(Guid actorId, Guid trackerId, DateTimeOffset eventHappensDate, EventCustomParameters customParameters)
        {
            var newEvent = new Event(Guid.NewGuid(), actorId, trackerId, eventHappensDate, customParameters);
            var tracker = _trackerRepository.LoadTracker(trackerId);
            if (!tracker.SettingsAndEventCustomizationsMatch(newEvent))
            {
                throw new Exception(); //todo return no result
            }
            
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

            var updatedEvent = new Event(eventId, tracker.Id, tracker.CreatorId, timeStamp, customParameters);
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
    }
}