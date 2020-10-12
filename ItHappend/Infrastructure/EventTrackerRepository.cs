using System;
using System.Collections.Generic;
using ItHappend.Domain;
using ItHappend.Domain.Exceptions;

namespace ItHappend.Infrastructure
{
    public class EventTrackerRepository : IEventTrackerRepository
    {
        private readonly Dictionary<Guid, EventTracker> _eventTrackers = new Dictionary<Guid, EventTracker>();
        public void SaveEventTracker(EventTracker newEventTracker)
        {
            _eventTrackers.Add(newEventTracker.Id, newEventTracker);
        }

        public EventTracker LoadEvent(Guid eventTrackerId)
        {
            if (!_eventTrackers.ContainsKey(eventTrackerId))
            {
                throw new EventTrackerNotFoundException(eventTrackerId);
            }

            return _eventTrackers[eventTrackerId];
        }

        public void DeleteEvent(Guid eventId)
        {
            _eventTrackers.Remove(eventId);
        }
    }
}