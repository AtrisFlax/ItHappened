using System;
using ItHappened.Application.Authentication;
using ItHappened.Domain;

namespace ItHappened.Application.Services.UserService
{
    public interface IUserService
    {
        UserWithToken Register(string loginName, string password);
        UserWithToken Authenticate(string loginName, string password);
    }
}