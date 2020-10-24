using System;

namespace ItHappened.Domain
{
    public class EventsInfoRange
    {
        public DateTimeOffset HappensDate { get; set; }
        public EventCustomParameters CustomParameters { get; set; }
    }
}