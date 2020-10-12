using System;

namespace ItHappend.Domain.Exceptions
{
    public class EventTrackerNotFoundException : System.Exception
    {
        public EventTrackerNotFoundException(Guid eventTrackerId) : base($"EventTracker with id {eventTrackerId} not found")
        {
        }
    }
}