using System;

namespace ItHappened.Api.Requests
{
    public class AddTrackingRequest
    {
        public Guid UserId { get; set; }
        public string TrackingName { get; set; }
        
    }
}