using System;

namespace ItHappend.Domain
{
    public interface IUserRepository
    {
        void SaveUser(UserAuthInfo userAuthInfo);
        UserAuthInfo LoadUserAuthInfo(Guid userId);
        UserAuthInfo LoadUserAuthInfo(string login);
    }
}