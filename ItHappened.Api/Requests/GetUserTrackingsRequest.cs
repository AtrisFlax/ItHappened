using System;

namespace ItHappened.Api.Requests
{
    public class GetUserTrackingsRequest
    {
        public GetUserTrackingsRequest(Guid id)
        {
            Id = id;
        }

        public Guid Id { get; }
    }
}