using System;
using ItHappened.Domain.User;

namespace ItHappened.Domain
{
    public interface IUserRepository
    {
        void SaveUser(User.User user);
        User.User TryLoadUserAuthInfo(Guid userId);
        UserInfo TryLoadUserInfo(Guid userId);
        User.User TryLoadUserAuthInfo(string login);
    }
}