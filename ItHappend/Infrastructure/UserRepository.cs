using System;
using System.Collections.Generic;
using System.Linq;
using ItHappend.Domain;

namespace ItHappend.Infrastructure
{
    public class UserRepository : IUserRepository
    {
        private readonly Dictionary<Guid, UserAuthInfo> _users = new Dictionary<Guid, UserAuthInfo>();
        public void TrySaveUser(UserAuthInfo newUser)
        {
            _users.Add(newUser.Guid, newUser);
        }

        public UserAuthInfo TryLoadUserAuthInfo(Guid userId)
        {
            return _users[userId];
        }

        public UserAuthInfo TryLoadUserAuthInfo(string login)
        {
            return _users
                .First(dictionaryItem => dictionaryItem.Value.Login == login)
                .Value;
        }
    }
}