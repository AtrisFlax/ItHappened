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

        public void AddEventToTracker(Guid trackerId, Guid eventId)
        {
            var requiredTracker = _eventTrackerRepository.LoadEventTracker(trackerId);
            var eventToAdd = _eventRepository.LoadEvent(eventId);
            requiredTracker.AddEvent(eventToAdd);
        }

        public void RemoveEventFromTracker(Guid trackerId, Guid eventId)
        {
            throw new NotImplementedException();
        }

        public void DeleteTracker(Guid trackerOwnerId, Guid tracker)
        {
            throw new NotImplementedException();
        }
    }
}