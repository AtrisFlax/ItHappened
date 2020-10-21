using System;
using ItHappened.Domain;

namespace ItHappened.Application.Services.UserService
{
    public interface IUserService
    {
        User Register(string name, string password);
        User TryFindByLogin(string login);
    }
}