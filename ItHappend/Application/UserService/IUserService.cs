using System;
using ItHappend.Domain;

namespace ItHappend.UserService
{
    public interface IUserService
    {
        public (Guid id, UserServiceStatusCodes status) RegisterUser(string login, string password);

        public (UserInfo userInfo, UserServiceStatusCodes status) AuthenticateUser(string login, string password);
    }
}