﻿using System.Collections.Generic;

namespace ItHappened.Api.Middleware
{
    public class ExceptionResponse
    {
        public ExceptionResponse(string errorMessage, Dictionary<string, object> payload = null)
        {
            ErrorMessage = errorMessage;
            Payload = payload;
        }

        public string ErrorMessage { get; }
        public Dictionary<string, object> Payload { get; }
    }
}