using System;
using System.Collections.Generic;
using System.Net;

namespace ItHappened.Application.Errors
{
    public class UserTrackersStatisticsNotFoundException : BusinessException
    {
        public UserTrackersStatisticsNotFoundException(Guid userId)
            : base(HttpStatusCode.NotFound,
                "Can't find statistics in a database",
                new Dictionary<string, object>{{"userId", userId}})
        {
        }
    }
}