﻿using System;

namespace ItHappened.Api.Contracts.Requests.Trackers
{
    public class UpdateTrackerRequest
    {
        public Guid UserId { get; set; }
        public string NewTrackerName { get; set; }
    }
}