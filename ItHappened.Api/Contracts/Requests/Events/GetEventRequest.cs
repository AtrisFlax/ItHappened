using System;

namespace ItHappened.Api.Contracts.Requests.Events
{
    public class GetEventRequest
    {
        public Guid UserId { get; set; }
        public Guid EventId { get; set; }
    }
}