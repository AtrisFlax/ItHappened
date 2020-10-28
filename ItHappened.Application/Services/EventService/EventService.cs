using System;
using System.Collections.Generic;
using System.Net;
using ItHappened.Application.Errors;
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

        public Guid CreateEvent(Guid actorId, Guid trackerId, DateTimeOffset eventHappensDate,
            EventCustomParameters customParameters)
        {
            if (!_trackerRepository.IsContainTracker(trackerId))
            {
                throw new RestException(HttpStatusCode.NotFound);
            }
            var tracker = _trackerRepository.LoadTracker(trackerId);
            if (tracker.CreatorId != actorId)
            {
                throw new RestException(HttpStatusCode.BadRequest);
            }
            
            var newEventId = Guid.NewGuid();
            while (_eventRepository.IsContainEvent(newEventId))
            {//if generated GUID already exist in repository -> regenerate
                newEventId = Guid.NewGuid();
            }
            
            var newEvent = new Event(newEventId, actorId, trackerId, eventHappensDate, customParameters);
            
            if (!tracker.IsTrackerCustomizationAndEventCustomizationMatch(newEvent))
            {
                throw new RestException(HttpStatusCode.BadRequest);
            }

            _eventRepository.SaveEvent(newEvent);
            tracker.IsUpdated = true;
            return newEvent.Id;
        }
    
        public Event GetEvent(Guid actorId, Guid eventId)
        {
            if (!_eventRepository.IsContainEvent(eventId))
            {
                throw new RestException(HttpStatusCode.NotFound);
            }
            
            var @event = _eventRepository.LoadEvent(eventId);
            if (actorId != @event.CreatorId)
            {
                throw new RestException(HttpStatusCode.BadRequest);
            }
            
            return @event;
        }

        public IReadOnlyCollection<Event> GetAllTrackerEvents(Guid actorId, Guid trackerId)
        {
            if (!_trackerRepository.IsContainTracker(trackerId))
            {
                throw new RestException(HttpStatusCode.NotFound);
            }
            
            var tracker = _trackerRepository.LoadTracker(trackerId);
            if (actorId != tracker.CreatorId) 
            {
                throw new RestException(HttpStatusCode.BadRequest);
            }
            
            return _eventRepository.LoadAllTrackerEvents(trackerId);
        }
        
        public IReadOnlyCollection<Event> GetAllFilteredEvents(Guid actorId, Guid trackerId, IEnumerable<IEventsFilter> eventsFilters)
        {
            return EventsFilter.Filter(GetAllTrackerEvents(actorId, trackerId), eventsFilters);
        }
        

        public void EditEvent(Guid actorId,
            Guid eventId,
            DateTimeOffset timeStamp,
            EventCustomParameters customParameters)
        {
            if (!_eventRepository.IsContainEvent(eventId))
            {
                throw new RestException(HttpStatusCode.NotFound);
            }
            
            var @event = _eventRepository.LoadEvent(eventId);
            if (actorId != @event.CreatorId)
            {
                throw new RestException(HttpStatusCode.BadRequest);
            }
            
            var tracker = _trackerRepository.LoadTracker(@event.TrackerId);
            //TODO: тут проверить соответствие кастомизации трекера и редактируемого события
            //оставляю до merge, т.к. там эти функции и поле трекера изменились
            
            var updatedEvent = new Event(eventId, @event.CreatorId, @event.TrackerId, timeStamp, customParameters);
            _eventRepository.UpdateEvent(updatedEvent);
            tracker.IsUpdated = true;
        }

        public void DeleteEvent(Guid actorId, Guid eventId)
        {
            if (!_eventRepository.IsContainEvent(eventId))
            {
                throw new RestException(HttpStatusCode.NotFound);
            }
            
            var @event = _eventRepository.LoadEvent(eventId);
            if (actorId != @event.CreatorId)
            {
                throw new RestException(HttpStatusCode.BadRequest);   
            }

            var tracker = _trackerRepository.LoadTracker(@event.TrackerId);
            _eventRepository.DeleteEvent(eventId);
            tracker.IsUpdated = true;
        }
    }
}