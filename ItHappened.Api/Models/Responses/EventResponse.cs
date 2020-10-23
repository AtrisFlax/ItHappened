using System;

namespace ItHappened.Api.Models.Responses
{
    public class EventResponse
    {
        public Guid Id { get; set; }
        public Guid CreatorId { get; set; }
        public Guid TrackerId { get; set; }
        public DateTimeOffset HappensDate { get; set; }
        public EventCustomParametersResponse CustomParameters { get; set; }
    }
}