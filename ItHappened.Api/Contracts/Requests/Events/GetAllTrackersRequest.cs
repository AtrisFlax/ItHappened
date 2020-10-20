using System;

namespace ItHappened.Api.Contracts.Requests.Events
{
    public class GetAllEventsRequest
    {
        public Guid UserId { get; set; }
        public Guid TrackerId { get; set; }
        public FiltrationType Filtration { get; set; }
    }

    public enum FiltrationType
    {
        DateTime, Score
    }
}