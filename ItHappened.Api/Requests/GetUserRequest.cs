using System;

namespace ItHappened.Api.Requests
{
    public class GetUserRequest
    {
        public GetUserRequest(Guid id)
        {
            Id = id;
        }

        public Guid Id { get; }
    }
}