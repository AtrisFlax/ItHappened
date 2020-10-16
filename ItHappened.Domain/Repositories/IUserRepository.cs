using System;
using ItHappened.Domain.User;

namespace ItHappened.Domain.Repositories
{
    public interface IUserRepository
    {
        void SaveUser(UserAuthInfo userAuthInfo);
        UserAuthInfo TryLoadUserAuthInfo(Guid userId);
        UserInfo TryLoadUserInfo(Guid userId);
        UserAuthInfo TryLoadUserAuthInfo(string login);
    }
}