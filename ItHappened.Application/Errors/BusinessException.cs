using System;
using System.Collections.Generic;
using System.Net;

namespace ItHappened.Application.Errors
{
    public abstract class BusinessException : Exception
    {
        //  TODO Dictionary<string, object> pass object bad pattern
        //  TODO create to overload constructors with string ang guid instead object
        protected BusinessException(HttpStatusCode httpErrorCode,
            string errorMessage,
            Dictionary<string, object> payload) 
        {
            HttpErrorCode = httpErrorCode;
            ErrorMessage = errorMessage;
            Payload = payload;
        }
        public HttpStatusCode HttpErrorCode { get; }
        public string ErrorMessage { get; }
        public Dictionary<string, object> Payload { get; }
    }
}