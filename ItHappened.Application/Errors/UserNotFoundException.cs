﻿using System.Collections.Generic;
using System.Net;

namespace ItHappened.Application.Errors
{
    public class UserNotFoundException : BusinessException
    {
        public UserNotFoundException(string loginName, string password)
            : base(HttpStatusCode.NotFound,
                "User with provided credentials not found",
                new Dictionary<string, object>{{"login", loginName}, {"password", password}})
        {
        }
    }
}