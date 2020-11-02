﻿using System;
using System.Security.Claims;
using ItHappened.Api.Authentication;

namespace ItHappened.Api
{
    public static class UserExtensions
    {
        public static Guid GetUserId(this ClaimsPrincipal user)
        {
            return Guid.Parse(user.FindFirstValue(JwtClaimTypes.Id));
        }
    }
}