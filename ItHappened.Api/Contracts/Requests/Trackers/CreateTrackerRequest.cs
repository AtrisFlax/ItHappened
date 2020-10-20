using System;

namespace ItHappened.Api.Contracts.Requests.Trackers
{
    public class CreateTrackerRequest
    {
        public Guid UserId { get; set; }
        public string TrackerName { get; set; }
    }
}