using System;
using System.Collections.Generic;

namespace ItHappened.Domain
{
    public interface ITrackerRepository
    {
        void SaveTracker(EventTracker newTracker);
        EventTracker LoadTracker(Guid eventTrackerId);
        IReadOnlyCollection<EventTracker> LoadAllUserTrackers(Guid userId);
        void UpdateTracker(EventTracker eventTracker);
        void DeleteTracker(Guid trackerId);
        bool IsContainTracker(Guid trackerId);
    }
}