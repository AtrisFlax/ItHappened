using System;
using System.Collections.Generic;

namespace ItHappened.Domain
{
    public interface IUserRepository
    {
        void SaveUser(User user);
        User LoadUser(Guid userId);
        User TryFindByLogin(string login);
        IEnumerable<Guid> LoadAllUsersIds();
    }
}