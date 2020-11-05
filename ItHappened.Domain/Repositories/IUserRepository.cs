using System;
using System.Collections.Generic;

namespace ItHappened.Domain
{
    public interface IUserRepository
    {
        void CreateUser(User user);
        User LoadUser(string loginName);
        bool HasUserWithLogin(string loginName);
        IEnumerable<Guid> LoadAllUsersIds();
    }
}