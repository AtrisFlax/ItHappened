using System;
using System.Collections.Generic;

namespace ItHappened.Bll.Domain.Repositories
{
    public interface IEventTrackerRepository
    {
        void SaveEventTracker(EventTracker newEventTracker);
        EventTracker LoadEventTracker(Guid eventTrackerId);
        IList<EventTracker> LoadUserTrackers(Guid userId);
        void DeleteEventTracker(Guid eventId);
    }
}