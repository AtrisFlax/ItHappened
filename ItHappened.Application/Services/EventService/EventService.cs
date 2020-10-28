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
            var newEvent = new Event(Guid.NewGuid(), actorId, trackerId, eventHappensDate, customParameters);
            
            if (!tracker.IsSettingsAndEventCustomizationsMatch(newEvent))
            {
                throw new RestException(HttpStatusCode.BadRequest);
            }

            _eventRepository.SaveEvent(newEvent);
            tracker.IsUpdated = true;
            return newEvent.Id;
        }

        public void AddRangeEvent(Guid actorId, Guid trackerId, IEnumerable<EventsInfoRange> eventsInfoRange)
        {
            var tracker = _trackerRepository.LoadTracker(trackerId);
            foreach (var eventInfo in eventsInfoRange)
            {
                var newEvent = new Event(Guid.NewGuid(), actorId, trackerId, eventInfo.HappensDate,
                    eventInfo.CustomParameters);
                if (tracker.IsSettingsAndEventCustomizationsMatch(newEvent))
                {
                    _eventRepository.SaveEvent(newEvent);
                    tracker.IsUpdated = true;
                } //if customization not match skip
            }
        }


        public Event GetEvent(Guid actorId, Guid eventId)
        {
            var @event = _eventRepository.LoadEvent(eventId);
            if (actorId != @event.CreatorId)
            {
                throw new RestException(HttpStatusCode.BadRequest);
            }
            return @event;
        }

        public IReadOnlyCollection<Event> GetAllFilteredEvents(Guid actorId, Guid trackerId, IEnumerable<IEventsFilter> eventsFilters)
        {
            return EventsFilter.Filter(GetAllEvents(actorId, trackerId), eventsFilters);
        }

        public IReadOnlyCollection<Event> GetAllEvents(Guid actorId, Guid trackerId)
        {
            var tracker = _trackerRepository.LoadTracker(trackerId);
            if (actorId != tracker.CreatorId) {
                throw new RestException(HttpStatusCode.BadRequest);
            }
            var events = _eventRepository.LoadAllTrackerEvents(trackerId);
            return events;
        }

        public Event EditEvent(Guid actorId,
            Guid eventId,
            DateTimeOffset timeStamp,
            EventCustomParameters customParameters)
        {
            var @event = _eventRepository.LoadEvent(eventId);
            if (actorId != @event.CreatorId) 
            {
                throw new RestException(HttpStatusCode.BadRequest);
            }
            var tracker = _trackerRepository.LoadTracker(@event.TrackerId);
            var updatedEvent = new Event(eventId, @event.CreatorId, @event.TrackerId, timeStamp, customParameters);
            _eventRepository.UpdateEvent(updatedEvent);
            tracker.IsUpdated = true;
            return updatedEvent;
        }

        public Event DeleteEvent(Guid actorId, Guid eventId)
        {
            var @event = _eventRepository.LoadEvent(eventId);
            if (actorId != @event.CreatorId)
            {
                throw new RestException(HttpStatusCode.BadRequest);   
            }

            var tracker = _trackerRepository.LoadTracker(@event.TrackerId);
            _eventRepository.DeleteEvent(eventId);
            tracker.IsUpdated = true;
            return @event;
        }
    }
}