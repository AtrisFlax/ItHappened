using System;
using System.Collections.Generic;
using System.Net;

namespace ItHappened.Application.Errors
{
    public class TrackerSettingsAndEventDontMatch : BusinessException
    {
        public TrackerSettingsAndEventDontMatch(Guid trackerId, Guid eventId)
            : base(HttpStatusCode.BadRequest,
                "Tracker customization settings and event customizations don't match",
                new Dictionary<string, object>{{"trackerId", trackerId}, {"eventId", eventId}})
        {
        }
    }
}