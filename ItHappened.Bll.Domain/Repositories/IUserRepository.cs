using System;
using ItHappened.Bll.Domain.User;

namespace ItHappened.Bll.Domain.Repositories
{
    public interface IUserRepository
    {
        void SaveUser(UserAuthInfo userAuthInfo);
        UserAuthInfo TryLoadUserAuthInfo(Guid userId);
        UserInfo TryLoadUserInfo(Guid userId);
        UserAuthInfo TryLoadUserAuthInfo(string login);
    }
}