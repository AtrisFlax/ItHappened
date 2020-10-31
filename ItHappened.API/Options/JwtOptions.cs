using System;

namespace ItHappened.Api.Options
{
    public class JwtOptions
    {
        public string Secret { get; set; }
        public TimeSpan ExpiresAfter { get; set; }
    }
}