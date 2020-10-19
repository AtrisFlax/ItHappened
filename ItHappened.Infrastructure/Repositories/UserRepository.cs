using System;
using System.Collections.Generic;
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

        public User LoadUser(Guid userId)
        {
            return _users[userId];
        }
    }
}