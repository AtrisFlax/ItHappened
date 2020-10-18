using System;
using System.Collections.Generic;
using System.Linq;
using ItHappened.Domain;

namespace ItHappened.Infrastructure.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly Dictionary<Guid, User> _users = new Dictionary<Guid, User>();
        public void SaveUser(User newUser)
        {
            _users.Add(newUser.Guid, newUser);
        }

        public User TryLoadUserAuthInfo(Guid userId)
        {
            return _users[userId];
        }

        public User LoadUser(Guid userId)
        {
            throw new NotImplementedException();
        }

        public User TryLoadUserAuthInfo(string login)
        {
            return _users
                .First(dictionaryItem => dictionaryItem.Value.Name == login)
                .Value;
        }
    }
}