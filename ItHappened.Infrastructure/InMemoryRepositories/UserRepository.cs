using System;
using System.Collections.Generic;
using ItHappened.Domain;
using LanguageExt;

namespace ItHappened.Infrastructure.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly Dictionary<Guid, User> _users = new Dictionary<Guid, User>();

        public void CreateUser(User newUser)
        {
            _users.Add(newUser.Id, newUser);
        }
        
        public Option<User> LoadUserByLogin(string login)
        {
            return _users
                .Find(x => x.Value.Name == login)
                .Match((keyValue) => keyValue.Value, Option<User>.None);
        }

        public IEnumerable<Guid> LoadAllUsersIds()
        {
            return _users.Keys;
        }
    }
}