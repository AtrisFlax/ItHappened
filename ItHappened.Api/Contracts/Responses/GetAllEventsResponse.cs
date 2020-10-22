using System.Collections.Generic;

namespace ItHappened.Api.Contracts.Responses
{
    public class GetAllEventsResponse
    {
        public IEnumerable<EventResponse> Trackers { get; set; }
    }
}