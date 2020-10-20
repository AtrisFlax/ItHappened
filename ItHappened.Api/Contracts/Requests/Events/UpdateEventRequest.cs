using System;

namespace ItHappened.Api.Contracts.Requests.Events
{
    public class UpdateEventRequest
    {
        public Guid UserId { get; set; }
        public Guid EventId { get; set; }
        public DateTimeOffset TimeStamp { get; set; }
    }
}