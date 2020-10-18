using System;

namespace ItHappened.Api.Responses
{
    public class CreateUserResponse
    {
        public CreateUserResponse(Guid id)
        {
            Id = id;
        }

        public Guid Id { get; }
    }
}