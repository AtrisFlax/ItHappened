using System;

namespace ItHappened.Api.Requests
{
    public class AddTrackingRequest
    {
        public Guid UserId { get; }
        public string TrackingName { get; }
    }
}