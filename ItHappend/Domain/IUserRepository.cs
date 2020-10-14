using System;

namespace ItHappend.Domain
{
    public interface IUserRepository
    {
        void SaveUser(UserAuthInfo userAuthInfo);
        UserAuthInfo TryLoadUserAuthInfo(Guid userId);
        UserAuthInfo TryLoadUserAuthInfo(string login);
    }
}