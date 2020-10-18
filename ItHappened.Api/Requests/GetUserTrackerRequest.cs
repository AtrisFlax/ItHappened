using System;

namespace ItHappened.Api.Requests
{
    public class GetUserTrackerRequest
    {
        public GetUserTrackerRequest(Guid id)
        {
            Id = id;
        }

        public Guid Id { get; }
    }
}