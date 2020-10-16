using System;
using System.Collections.Generic;

namespace ItHappened.Domain.Repositories
{
    public interface IEventTrackerRepository
    {
        void SaveEventTracker(EventTracker.EventTracker newEventTracker);
        EventTracker.EventTracker LoadEventTracker(Guid eventTrackerId);
        IList<EventTracker.EventTracker> LoadUserTrackers(Guid userId);
        void DeleteEventTracker(Guid eventId);
    }
}