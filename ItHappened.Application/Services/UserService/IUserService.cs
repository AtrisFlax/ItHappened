using System;
using ItHappened.Domain;

namespace ItHappened.Application.Services.UserService
{
    public interface IUserService
    {
        Guid CreateUser(string name);
        User GetUser(Guid id);
    }
}