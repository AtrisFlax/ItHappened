using System.Collections.Generic;

namespace ItHappened.Api.Contracts.Responses
{
    public class GetAllTrackersResponse
    {
        public IEnumerable<TrackerResponse> Trackers { get; set; }
    }
}