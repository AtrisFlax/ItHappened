﻿using System;
using System.Collections.Generic;
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
                throw new TrackerNotFoundException(trackerId);
            }
            var tracker = _trackerRepository.LoadTracker(trackerId);
            if (tracker.CreatorId != actorId)
            {
                throw new NoPermissionsForTrackerException(actorId, trackerId);
            }
            
            var newEventId = Guid.NewGuid();
            while (_eventRepository.IsContainEvent(newEventId))
            {//if generated GUID already exist in repository -> regenerate
                newEventId = Guid.NewGuid();
            }
            
            var newEvent = new Event(newEventId, actorId, trackerId, eventHappensDate, customParameters);

            if (tracker.CustomizationSettings.IsCustomizationRequired &&
                !tracker.IsTrackerCustomizationAndEventCustomizationMatch(newEvent))
            {
                throw new TrackerSettingsAndEventDontMatch(trackerId, newEventId);
            }

            _eventRepository.SaveEvent(newEvent);
            tracker.IsUpdated = true;
            return newEvent.Id;
        }
    
        public Event GetEvent(Guid actorId, Guid eventId)
        {
            if (!_eventRepository.IsContainEvent(eventId))
            {
                throw new EventNotFoundException(eventId);
            }
            
            var @event = _eventRepository.LoadEvent(eventId);
            if (actorId != @event.CreatorId)
            {
                throw new NoPermissionsForEventException(actorId, eventId);
            }
            
            return @event;
        }

        public IReadOnlyCollection<Event> GetAllTrackerEvents(Guid actorId, Guid trackerId)
        {
            if (!_trackerRepository.IsContainTracker(trackerId))
            {
                throw new TrackerNotFoundException(trackerId);
            }
            
            var tracker = _trackerRepository.LoadTracker(trackerId);
            if (actorId != tracker.CreatorId) 
            {
                throw new NoPermissionsForTrackerException(actorId, trackerId);
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
                throw new EventNotFoundException(eventId);
            }
            
            var @event = _eventRepository.LoadEvent(eventId);
            if (actorId != @event.CreatorId)
            {
                throw new NoPermissionsForEventException(actorId, eventId);
            }
            
            var tracker = _trackerRepository.LoadTracker(@event.TrackerId);
            var updatedEvent = new Event(eventId, @event.CreatorId, @event.TrackerId, timeStamp, customParameters);
            if (tracker.CustomizationSettings.IsCustomizationRequired &&
                !tracker.IsTrackerCustomizationAndEventCustomizationMatch(updatedEvent))
            {
                throw new TrackerSettingsAndEventDontMatch(tracker.Id, eventId);
            }
            
            _eventRepository.UpdateEvent(updatedEvent);
            tracker.IsUpdated = true;
        }

        public void DeleteEvent(Guid actorId, Guid eventId)
        {
            if (!_eventRepository.IsContainEvent(eventId))
            {
                throw new EventNotFoundException(eventId);
            }
            
            var @event = _eventRepository.LoadEvent(eventId);
            if (actorId != @event.CreatorId)
            {
                throw new NoPermissionsForEventException(actorId, eventId);
            }

            var tracker = _trackerRepository.LoadTracker(@event.TrackerId);
            _eventRepository.DeleteEvent(eventId);
            tracker.IsUpdated = true;
        }
    }
}