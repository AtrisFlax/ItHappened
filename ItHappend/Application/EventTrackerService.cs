using System;
using System.Collections.Generic;
using ItHappend.Domain;

namespace ItHappend.Application
{
    public class EventTrackerService
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

        public Guid CreateTracker(Guid creatorId)
        {
            var creator = _userRepository.LoadUserAuthInfo(creatorId);
            var newTrackerId = Guid.NewGuid();
            var eventTracker = new EventTracker(
                newTrackerId,
                new List<Event>(),
                creatorId
                );
            return newTrackerId;
        }

        public void AddEventToTracker(Guid trackerId, Guid eventId, Guid initiatorId)
        {
            var requiredTracker = _eventTrackerRepository.LoadEventTracker(trackerId);
            if (initiatorId != requiredTracker.CreatorId)
            {
                throw new Exception();
            }
            var eventToAdd = _eventRepository.LoadEvent(eventId);
            requiredTracker.AddEvent(eventToAdd);
        }

        public void RemoveEventFromTracker(Guid trackerId, Guid eventId, Guid initiatorId)
        {
            var requiredTracker = _eventTrackerRepository.LoadEventTracker(trackerId);
            if (initiatorId != requiredTracker.CreatorId)
            {
                throw new Exception();
            }
            var eventToRemove = _eventRepository.LoadEvent(eventId);
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
            
        }
    }
}