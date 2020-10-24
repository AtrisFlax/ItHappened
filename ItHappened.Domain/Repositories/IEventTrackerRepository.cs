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
        bool DeleteTracker(Guid eventId);
        IEnumerable<EventTracker> LoadAllTrackers();
    }
}