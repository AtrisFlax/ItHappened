using System;
using System.Collections.Generic;
using System.Net;

namespace ItHappened.Application.Errors
{
    public class EventNotFoundException : BusinessException
    {
        public EventNotFoundException(Guid eventId)
            : base(HttpStatusCode.NotFound,
                "Can't find event in a database",
                new Dictionary<string, object>{{"eventId", eventId}})
        {
        }
    }
}