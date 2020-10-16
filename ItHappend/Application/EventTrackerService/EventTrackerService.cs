using System;
using System.Collections.Generic;
using ItHappend.Domain;

namespace ItHappend.EventTrackerService
{
    public class EventTrackerService : IEventTrackerService
    {
        private readonly IEventTrackerRepository _eventTrackerRepository;
        private readonly IUserRepository _userRepository;
        private readonly IEventRepository _eventRepository;

        public EventTrackerService(IEventTrackerRepository eventTrackerRepository, 
            IUserRepository userRepository, 
            IEventRepository eventRepository)
        {
            _eventTrackerRepository = eventTrackerRepository;
            _userRepository = userRepository;
            _eventRepository = eventRepository;
        }

        public Guid CreateTracker(Guid creatorId, string trackerName)
        {
            var creator = _userRepository.TryLoadUserAuthInfo(creatorId);
            var newTrackerId = Guid.NewGuid();
            var eventTracker = new EventTracker(
                newTrackerId,
                trackerName, 
                new List<Event>(),
                creatorId
                );
            return newTrackerId;
        }

        public bool TryAddEventToTracker(Guid trackerId, Guid eventId, Guid initiatorId)
        {
            var requiredTracker = _eventTrackerRepository.LoadEventTracker(trackerId);
            if (initiatorId != requiredTracker.CreatorId)
            {
                return false;
            }
            var eventToAdd = _eventRepository.TryLoadEvent(eventId);
            return requiredTracker.TryAddEvent(eventToAdd);
        }

        public void RemoveEventFromTracker(Guid trackerId, Guid eventId, Guid initiatorId)
        {
            var requiredTracker = _eventTrackerRepository.LoadEventTracker(trackerId);
            if (initiatorId != requiredTracker.CreatorId)
            {
                throw new Exception();
            }
            var eventToRemove = _eventRepository.TryLoadEvent(eventId);
            requiredTracker.RemoveEvent(eventToRemove);
        }

        public void DeleteTracker(Guid trackerId, Guid initiatorId)
        {
            var requiredTracker = _eventTrackerRepository.LoadEventTracker(trackerId);
            if (initiatorId != requiredTracker.CreatorId)
            {
                throw new Exception();
            }
            _eventTrackerRepository.DeleteEventTracker(trackerId);
        }

        public IReadOnlyCollection<Event> FilterTrackerEventsByTimeSpan(Guid trackerId, Guid initiatorId, 
            DateTimeOffset from, DateTimeOffset to)
        {
            var requiredTracker = _eventTrackerRepository.LoadEventTracker(trackerId);
            if (initiatorId != requiredTracker.CreatorId)
            {
                throw new Exception();
            }

            return requiredTracker.FilterEventsByTimeSpan(from, to);
        }
    }
}