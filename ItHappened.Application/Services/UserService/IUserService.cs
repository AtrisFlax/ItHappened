using System;
using ItHappened.Domain.User;
using LanguageExt;

namespace ItHappened.Application.Services.UserService
{
    public interface IUserService
    {
        Guid CreateUser(string name);
    }
}