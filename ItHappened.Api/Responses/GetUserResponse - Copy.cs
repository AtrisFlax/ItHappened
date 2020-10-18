using System;

namespace ItHappened.Api.Responses
{
    public class GetUserResponse
    {
        public GetUserResponse(string name)
        {
            Name = name;
        }

        public string Name { get; }
    }
}