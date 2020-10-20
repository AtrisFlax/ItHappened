using System;

namespace ItHappened.Api.Contracts.Responses.User
{
    public class CreateUserResponse
    {
        public CreateUserResponse(Guid userId)
        {
            UserId = userId;
        }
        public Guid UserId { get; }
    }
}