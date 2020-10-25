using System;
using ItHappened.Api.Authentication;
using ItHappened.Domain;

namespace ItHappened.Application.Services.UserService
{
    public interface IUserService
    {
        User Register(string loginName, string password);
        Token Authenticate(string loginName, string password);
    }
}