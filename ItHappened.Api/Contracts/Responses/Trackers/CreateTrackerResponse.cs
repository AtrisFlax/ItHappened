using System;

namespace ItHappened.Api.Contracts.Responses.Trackers
{
    public class CreateTrackerResponse
    {
        public CreateTrackerResponse(Guid trackerId)
        {
            TrackerId = trackerId; 
        }
        public Guid TrackerId { get; set; }
    }
}