using System;

namespace ItHappened.Domain
{
    public interface IUserRepository
    {
        void SaveUser(User user);
        User LoadUser(Guid userId);
    }
}