using System;
using System.Collections.Generic;
using System.Linq;
using ItHappend.Domain.Exceptions;

namespace ItHappend.Infrastructure
{
    public class UserRepository : IUserRepository
    {
        private readonly Dictionary<Guid, UserAuthInfo> _users = new Dictionary<Guid, UserAuthInfo>();
        public void AddUser(UserAuthInfo newUser)
        {
            _users.Add(newUser.Guid, newUser);
        }

        public UserAuthInfo GetUser(Guid userId)
        {
            if (!_users.ContainsKey(userId))
            {
                throw new UserNotFoundException(userId);
            }

            return _users[userId];
        }

        public UserAuthInfo GetAuthInfo(string login)
        {
            var isUserExist = _users
                .Any(dictionaryItem => dictionaryItem.Value.Login == login);
            if (!isUserExist)
            {
                throw new UserNotFoundException(login);
            }

            return _users
                .First(dictionaryItem => dictionaryItem.Value.Login == login)
                .Value;
        }
    }
}