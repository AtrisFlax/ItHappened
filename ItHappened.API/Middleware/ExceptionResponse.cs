﻿using System;
 using System.Collections.Generic;

namespace ItHappened.Api.Middleware
{
    public class ExceptionResponse
    {
        public ExceptionResponse(string errorMessage, string exceptionType = null, Dictionary<string, object> payload = null)
        {
            ErrorMessage = errorMessage;
            ExceptionType = exceptionType;
            Payload = payload;
        }
        
        public string ExceptionType { get; }
        public string ErrorMessage { get; }
        public Dictionary<string, object> Payload { get; }
    }
}