using System;

namespace ItHappend
{
    interface IUserRepository
    {
        void AddUser(UserAuthInfo userAuthInfo);
        Guid GetUser(string login, object securePasswordHasher);
        (string userId, string hash) GetAuthInfo(string login);
    }
}