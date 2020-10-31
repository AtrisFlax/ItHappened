using System;
using System.Collections.Generic;
using System.Net;

namespace ItHappened.Application.Errors
{
    public class TrackerNotFoundException : BusinessException
    {
        public TrackerNotFoundException(Guid trackerId)
            : base(HttpStatusCode.NotFound,
                "Can't find event tracker in a database",
                new Dictionary<string, object>{{"trackerId", trackerId}})
        {
        }
    }
}