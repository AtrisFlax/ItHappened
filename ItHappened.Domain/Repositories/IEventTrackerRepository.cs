using System;
using System.Collections.Generic;

namespace ItHappened.Domain
{
    public interface IEventTrackerRepository
    {
        bool IsContainTracker(Guid trackerId);
        void SaveTracker(EventTracker newTracker);
        EventTracker LoadTracker(Guid eventTrackerId);
        IEnumerable<EventTracker> LoadAllUserTrackers(Guid userId);
        void UpdateTracker(EventTracker eventTracker);
        void DeleteTracker(Guid eventTrackerId);
    }
}