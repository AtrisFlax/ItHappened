using System;
using System.Collections.Generic;
using System.Linq;
using ItHappened.Domain;

namespace ItHappened.Infrastructure.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly Dictionary<Guid, User> _users = new Dictionary<Guid, User>();

        public void CreateUser(User newUser)
        {
            _users.Add(newUser.Id, newUser);
        }
        
        public User TryFindByLogin(string login)
        {
            return _users
                .FirstOrDefault(x => x.Value.Name == login)
                .Value;
        }

        public IEnumerable<Guid> LoadAllUsersIds()
        {
            return _users.Keys;
        }
    }
}