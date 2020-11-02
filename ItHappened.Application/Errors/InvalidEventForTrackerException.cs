﻿using System;
using System.Collections.Generic;
using System.Net;

namespace ItHappened.Application.Errors
{
    public class InvalidEventForTrackerException : ValidationException
    {
        public InvalidEventForTrackerException(Guid trackerId)
            : base("Tracker customization settings and event customizations don't match",
                new Dictionary<string, object>{{"trackerId", trackerId}})
        {
        }
    }
}