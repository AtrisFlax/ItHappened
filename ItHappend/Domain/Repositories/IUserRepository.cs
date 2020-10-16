using System;

namespace ItHappend.Domain
{
    public interface IUserRepository
    {
        void SaveUser(UserAuthInfo userAuthInfo);
        UserAuthInfo TryLoadUserAuthInfo(Guid userId);
        UserInfo TryLoadUserInfo(Guid userId);
        UserAuthInfo TryLoadUserAuthInfo(string login);
    }
}