using System;

namespace ItHappened.Api.Authentication
{
    public class JwtConfiguration
    {
        public string Secret { get; set; }
        
        public TimeSpan ExpiresAfter { get; set; }
    }
}