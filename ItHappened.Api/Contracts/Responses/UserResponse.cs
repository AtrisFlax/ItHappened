using System;
using ItHappened.Domain;

namespace ItHappened.Api.Contracts.Responses
{
    public class UserResponse
    {
        public UserResponse(Guid id, string name, string token)
        {
            Id = id;
            Name = name;
            Token = token;
        }
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Token { get; set; }
    }
}