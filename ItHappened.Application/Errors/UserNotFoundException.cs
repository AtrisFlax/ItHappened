﻿using System.Collections.Generic;
using System.Net;

namespace ItHappened.Application.Errors
{
    public class UserNotFoundException : BusinessException
    {
        public UserNotFoundException(string loginName, string password)
            : base(HttpStatusCode.NotFound,
                "Login or password provided were incorrect!",
                new Dictionary<string, object>{{"login", loginName}, {"password", password}})
        {
        }
    }
}
