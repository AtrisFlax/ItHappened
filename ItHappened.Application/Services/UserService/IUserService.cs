using System;
using ItHappened.Domain;

namespace ItHappened.Application.Services.UserService
{
    public interface IUserService
    {
        void Register(string login, string password);
        User TryFindByLogin(string login);
    }
}