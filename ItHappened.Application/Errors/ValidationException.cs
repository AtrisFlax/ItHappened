using System.Collections.Generic;
using System.Net;

namespace ItHappened.Application.Errors
{
    public class ValidationException : BusinessException
    {
        public ValidationException(string errorMessage,
            Dictionary<string, object> payload) : base(HttpStatusCode.BadRequest, errorMessage, payload)
        {
        }
    }
}