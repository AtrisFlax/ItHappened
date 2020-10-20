using System;

namespace ItHappened.Api.Contracts.Requests.Trackers
{
    public class GetAllTrackersRequest
    {
        public Guid UserId { get; set; }
    }
}