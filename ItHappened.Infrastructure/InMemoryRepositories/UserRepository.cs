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
        
        public User LoadUser(string loginName)
        {
            return _users.FirstOrDefault(x => x.Value.Name == loginName).Value;
        }

        public bool HasUserWithLogin(string loginName)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Guid> LoadAllUsersIds()
        {
            return _users.Keys;
        }
    }
}