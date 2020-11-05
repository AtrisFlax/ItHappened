using System;
using System.Collections.Generic;
using LanguageExt;

namespace ItHappened.Domain
{
    public interface IUserRepository
    {
        void CreateUser(User user);
        Option<User> LoadUserByLogin(string login);
        IEnumerable<Guid> LoadAllUsersIds();
    }
}