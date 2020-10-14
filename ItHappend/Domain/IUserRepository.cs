using System;

namespace ItHappend.Domain
{
    interface IUserRepository
    {
        void TrySaveUser(UserAuthInfo userAuthInfo);
        UserAuthInfo TryLoadUserAuthInfo(Guid userId);
        UserAuthInfo TryLoadUserAuthInfo(string login);
    }
}