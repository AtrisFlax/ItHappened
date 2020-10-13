using System;
using System.Collections.Generic;

namespace ItHappend.Domain
{
    interface IEventTrackerRepository
    {
        void SaveEventTracker(EventTracker newEventTracker);
        EventTracker LoadEventTracker(Guid eventTrackerId);
        IList<EventTracker> LoadUserTrackers(Guid userId);
        void DeleteEventTracker(Guid eventId);
    }
}