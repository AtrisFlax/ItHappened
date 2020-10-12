using System;

namespace ItHappend.Domain.Exceptions
{
    public class EventNotFoundException : System.Exception
    {
        public EventNotFoundException(Guid eventId) : base($"Event with id {eventId} not found")
        {
        }
    }
}