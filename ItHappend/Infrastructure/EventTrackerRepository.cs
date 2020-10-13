using System;
using System.Collections.Generic;
using ItHappend.Domain;
using ItHappend.Domain.Exceptions;

namespace ItHappend.Infrastructure
{
    public class EventTrackerRepository : IEventTrackerRepository
    {
        private readonly Dictionary<Guid, EventTracker> _eventTrackers = new Dictionary<Guid, EventTracker>();
        public void AddEventTracker(EventTracker newEventTracker)
        {
            _eventTrackers.Add(newEventTracker.Id, newEventTracker);
        }

        public EventTracker GetEventTracker(Guid eventTrackerId)
        {
            if (!_eventTrackers.ContainsKey(eventTrackerId))
            {
                throw new EventTrackerNotFoundException(eventTrackerId);
            }

            return _eventTrackers[eventTrackerId];
        }

        public void DeleteEventTracker(Guid eventId)
        {
            _eventTrackers.Remove(eventId);
        }
    }
}