using System;
using System.Collections.Generic;
using System.Linq;
using ItHappened.Domain.Repositories;
using ItHappened.Domain.User;

namespace ItHappened.Infrastructure.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly Dictionary<Guid, UserAuthInfo> _users = new Dictionary<Guid, UserAuthInfo>();
        public void SaveUser(UserAuthInfo newUser)
        {
            _users.Add(newUser.Guid, newUser);
        }

        public UserAuthInfo TryLoadUserAuthInfo(Guid userId)
        {
            return _users[userId];
        }

        public UserInfo TryLoadUserInfo(Guid userId)
        {
            throw new NotImplementedException();
        }

        public UserAuthInfo TryLoadUserAuthInfo(string login)
        {
            return _users
                .First(dictionaryItem => dictionaryItem.Value.Login == login)
                .Value;
        }
    }
}