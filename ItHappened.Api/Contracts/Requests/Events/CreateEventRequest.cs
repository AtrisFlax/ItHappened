using System;

namespace ItHappened.Api.Contracts.Requests.Events
{
    public class CreateEventRequest
    {
        public Guid UserId { get; set; }
        public Guid TrackerId { get; set; }
        public DateTimeOffset TimeStamp { get; set; }
    }
}