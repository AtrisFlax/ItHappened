using System;

namespace ItHappend.Domain
{
    interface IUserRepository
    {
        void SaveUser(UserAuthInfo userAuthInfo);
        UserAuthInfo LoadUserAuthInfo(Guid userId);
        UserAuthInfo LoadUserAuthInfo(string login);
    }
}