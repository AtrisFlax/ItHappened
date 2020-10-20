using System;
using System.Collections.Generic;

namespace ItHappened.Api.Contracts.Responses.Trackers
{
    public class GetAllTrackersResponse
    {
        public IEnumerable<GetTrackerResponse> Trackers { get; set; }
    }
}