using System;
using System.Collections.Generic;
using System.Net;

namespace ItHappened.Application.Errors
{
    public class TrackerStatisticsNotFoundException : BusinessException
    {
        public TrackerStatisticsNotFoundException(Guid trackerId)
            : base(HttpStatusCode.NotFound,
                "Can't find tracker statistics in a database",
                new Dictionary<string, object>{{"trackerId", trackerId}})
        {
        }
    }
}