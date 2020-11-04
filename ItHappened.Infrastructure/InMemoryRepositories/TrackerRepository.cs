using System;
using System.Collections.Generic;
using System.Linq;
using ItHappened.Domain;

namespace ItHappened.Infrastructure.Repositories
{
    public class TrackerRepository : ITrackerRepository
    {
        private readonly Dictionary<Guid, EventTracker> _eventTrackers = new Dictionary<Guid, EventTracker>();

        public void SaveTracker(EventTracker newTracker)
        {
            _eventTrackers[newTracker.Id] = newTracker;
        }

        public EventTracker LoadTracker(Guid eventTrackerId)
        {
            return _eventTrackers[eventTrackerId];
        }

        public bool IsContainTracker(Guid trackerId)
        {
            return _eventTrackers.ContainsKey(trackerId);
        }

        public bool IsExistTrackerWithSameName(Guid creatorId, string trackerName)
        {
            if (!_eventTrackers.Any())
            {
                return false;
            }
            return _eventTrackers
                .All(tracker => tracker.Value.CreatorId == creatorId && tracker.Value.Name == trackerName);
        }

        public IReadOnlyCollection<EventTracker> LoadAllUserTrackers(Guid userId)
        {
            return _eventTrackers
                .Values.Where(tracker => tracker.CreatorId == userId).ToList();
        }

        public void UpdateTracker(EventTracker eventTracker)
        {
            _eventTrackers[eventTracker.Id] = eventTracker;
        }

        void ITrackerRepository.DeleteTracker(Guid trackerId)
        {
            _eventTrackers.Remove(trackerId);
        }

        public IEnumerable<EventTracker> LoadAllTrackers()
        {
            return _eventTrackers.Values;
        }
    }
}