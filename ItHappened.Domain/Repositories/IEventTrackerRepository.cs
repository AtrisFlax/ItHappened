using System;
using System.Collections.Generic;

namespace ItHappened.Domain
{
    public interface IEventTrackerRepository
    {
        void SaveEventTracker(EventTracker newEventTracker);
        EventTracker LoadEventTracker(Guid eventTrackerId);
        IEnumerable<EventTracker> LoadUserTrackers(Guid userId);
        void DeleteEventTracker(Guid eventId);
    }
}