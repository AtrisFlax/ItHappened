using System;
using System.Collections.Generic;
using ItHappened.Domain;

namespace ItHappened.Infrastructure.EFCoreRepositories
{
    public class EFTrackerRepository : ITrackerRepository
    {
        public void SaveTracker(EventTracker newTracker)
        {
            throw new NotImplementedException();
        }

        public EventTracker LoadTracker(Guid eventTrackerId)
        {
            throw new NotImplementedException();
        }

        public IReadOnlyCollection<EventTracker> LoadAllUserTrackers(Guid userId)
        {
            throw new NotImplementedException();
        }

        public void UpdateTracker(EventTracker eventTracker)
        {
            throw new NotImplementedException();
        }

        public void DeleteTracker(Guid eventTrackerId)
        {
            throw new NotImplementedException();
        }

        public bool IsContainTracker(Guid trackerId)
        {
            throw new NotImplementedException();
        }
    }
}