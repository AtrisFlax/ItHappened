using System;
using System.Collections.Generic;
using ItHappened.Application.Errors;
using ItHappened.Domain;

namespace ItHappened.Application.Services.EventService
{
    public class EventService : IEventService
    {
        private readonly ITrackerRepository _trackerRepository;
        private readonly IEventRepository _eventRepository;
        private readonly IEventFiltrationRepository _eventFiltrationRepository;

        public EventService(IEventRepository eventRepository, ITrackerRepository trackerRepository,
            IEventFiltrationRepository eventFiltrationRepository)
        {
            _eventRepository = eventRepository;
            _trackerRepository = trackerRepository;
            _eventFiltrationRepository = eventFiltrationRepository;
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

            var newEvent = new Event(Guid.NewGuid(), actorId, trackerId, eventHappensDate, customParameters);

            if (!tracker.IsTrackerCustomizationAndEventCustomizationMatch(newEvent))
            {
                throw new InvalidEventForTrackerException(trackerId);
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
                throw new InvalidEventForTrackerException(tracker.Id);
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

        public IReadOnlyCollection<Event> GetAllFilteredEvents(Guid userId, Guid trackerId, EventFilterData eventFilter)
        {
            return _eventFiltrationRepository.GetAllFilteredEvents(userId, trackerId, eventFilter);
        }
    }
}