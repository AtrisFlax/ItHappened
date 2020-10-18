using System;

namespace ItHappened.Api.Responses
{
    public class GetUserResponse
    {
        public GetUserResponse(Guid id)
        {
            Id = id;
        }

        public Guid Id { get; }
    }
}