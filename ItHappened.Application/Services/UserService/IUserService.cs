using System;
using ItHappened.Domain.User;

namespace ItHappened.Application.Services.UserService
{
    public interface IUserService
    {
        public (Guid id, UserServiceStatusCodes status) RegisterUser(string login, string password);

        public (UserInfo userInfo, UserServiceStatusCodes status) AuthenticateUser(string login, string password);
    }
}