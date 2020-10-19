using System;
using System.Collections.Generic;
using System.Linq;
using ItHappened.Domain;

namespace ItHappened.Infrastructure.Repositories
{
    public class EventTrackerRepository : IEventTrackerRepository
    {
        private readonly Dictionary<Guid, EventTracker> _eventTrackers = new Dictionary<Guid, EventTracker>();

        public void SaveEventTracker(EventTracker newEventTracker)
        {
            _eventTrackers.Add(newEventTracker.TrackerId, newEventTracker);
        }

        public EventTracker LoadEventTracker(Guid eventTrackerId)
        {
            return _eventTrackers[eventTrackerId];
        }

        public IEnumerable<EventTracker> LoadUserTrackers(Guid userId)
        {
            return _eventTrackers
                .Values.Where(tracker => tracker.CreatorId == userId).ToList();
        }

        public void DeleteEventTracker(Guid eventId)
        {
            _eventTrackers.Remove(eventId);
        }
    }
}