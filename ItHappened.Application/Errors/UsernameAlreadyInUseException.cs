﻿using System.Collections.Generic;
using System.Net;

namespace ItHappened.Application.Errors
{
    public class UsernameAlreadyInUseException : BusinessException
    {
        public UsernameAlreadyInUseException(string username)
            : base(HttpStatusCode.BadRequest,
                "Username is already in use",
                new Dictionary<string, object>{{"username", username}})
        {
        }
    }
}