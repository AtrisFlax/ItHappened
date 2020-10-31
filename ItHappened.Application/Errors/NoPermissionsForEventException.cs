using System;
using System.Collections.Generic;
using System.Net;

namespace ItHappened.Application.Errors
{
    public class NoPermissionsForEventException : BusinessException
    {
        public NoPermissionsForEventException(Guid actorId, Guid eventId)
            : base(HttpStatusCode.BadRequest,
                "No permissions for operations with event",
                new Dictionary<string, object>{{"actorId", actorId}, {"eventId", eventId}})
        {
        }
    }
}