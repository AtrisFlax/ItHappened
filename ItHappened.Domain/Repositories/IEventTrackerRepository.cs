using System;
using System.Collections.Generic;

namespace ItHappened.Domain
{
    public interface IEventTrackerRepository
    {
        void SaveEventInTracker(EventTracker newEventTracker);
        EventTracker LoadEventFromTracker(Guid eventTrackerId);
        IEnumerable<EventTracker> LoadUserTrackers(Guid userId);
        void DeleteEventTracker(Guid eventId);
    }
}