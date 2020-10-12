using System;

namespace ItHappend
{
    interface IUserRepository
    {
        void AddUser(UserAuthInfo userAuthInfo);
        Guid GetUser(string login, object securePasswordHasher);
        UserAuthInfo GetAuthInfo(string login);
    }
}