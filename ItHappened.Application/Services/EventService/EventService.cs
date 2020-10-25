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

        public Event AddEvent(Guid actorId, Guid trackerId, DateTimeOffset eventHappensDate,
            EventCustomParameters customParameters)
        {
            var newEvent = new Event(Guid.NewGuid(), actorId, trackerId, eventHappensDate, customParameters);
            var tracker = _trackerRepository.LoadTracker(trackerId);
            if (!tracker.SettingsAndEventCustomizationsMatch(newEvent))
                throw new RestException(HttpStatusCode.BadRequest);

            _eventRepository.AddEvent(newEvent);
            return newEvent;
        }

        public void AddRangeEvent(Guid actorId, Guid trackerId, IEnumerable<EventsInfoRange> eventsInfoRange)
        {
            var tracker = _trackerRepository.LoadTracker(trackerId);
            foreach (var eventInfo in eventsInfoRange)
            {
                var newEvent = new Event(Guid.NewGuid(), actorId, trackerId, eventInfo.HappensDate,
                    eventInfo.CustomParameters);
                if (tracker.SettingsAndEventCustomizationsMatch(newEvent))
                {
                    _eventRepository.AddEvent(newEvent);
                } //if customization not match skip
            }
        }


        public Event GetEvent(Guid actorId, Guid eventId)
        {
            var @event = _eventRepository.LoadEvent(eventId);
            if (actorId != @event.CreatorId)
                throw new RestException(HttpStatusCode.BadRequest);
            return @event;
        }

        public IReadOnlyCollection<Event> GetAllFilteredEvents(Guid actorId, Guid trackerId, IEnumerable<IEventsFilter> eventsFilters)
        {
            return EventsFilter.Filter(GetAllEvents(actorId, trackerId), eventsFilters);
        }

        public IReadOnlyCollection<Event> GetAllEvents(Guid actorId, Guid trackerId)
        {
            var tracker = _trackerRepository.LoadTracker(trackerId);
            if (actorId != tracker.CreatorId)
                throw new RestException(HttpStatusCode.BadRequest);
            var events = _eventRepository.LoadAllTrackerEvents(trackerId);
            return events;
        }

        public Event EditEvent(Guid actorId,
            Guid eventId,
            DateTimeOffset timeStamp,
            EventCustomParameters customParameters)
        {
            var tracker = _eventRepository.LoadEvent(eventId);
            if (actorId != tracker.CreatorId)
                throw new RestException(HttpStatusCode.BadRequest);

            var updatedEvent = new Event(eventId, tracker.Id, tracker.CreatorId, timeStamp, customParameters);
            _eventRepository.UpdateEvent(updatedEvent);
            return updatedEvent;
        }

        public Event DeleteEvent(Guid actorId, Guid eventId)
        {
            var @event = _eventRepository.LoadEvent(eventId);
            if (actorId != @event.CreatorId)
                throw new RestException(HttpStatusCode.BadRequest);

            _eventRepository.DeleteEvent(eventId);
            return @event;
        }
    }
}