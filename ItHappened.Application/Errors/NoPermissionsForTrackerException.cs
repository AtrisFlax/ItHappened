using System;
using System.Collections.Generic;
using System.Net;

namespace ItHappened.Application.Errors
{
    public class NoPermissionsForTrackerException : BusinessException
    {
        public NoPermissionsForTrackerException(Guid actorId, Guid trackerId)
            : base(HttpStatusCode.BadRequest,
                "No permissions for operations with tracker",
                new Dictionary<string, object>{{"actorId", actorId}, {"trackerId", trackerId}})
        {
        }
    }
}