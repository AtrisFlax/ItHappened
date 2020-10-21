using System.Collections.Generic;
using ItHappened.Api.Contracts.Responses.Trackers;

namespace ItHappened.Api.Contracts.Responses.Events
{
    public class GetAllEventsResponse
    {
        public IEnumerable<GetEventResponse> Trackers { get; set; }
    }
}