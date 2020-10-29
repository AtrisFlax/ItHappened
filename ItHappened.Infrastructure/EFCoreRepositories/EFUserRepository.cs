using System;
using System.Collections.Generic;
using ItHappened.Domain;

namespace ItHappened.Infrastructure.EFCoreRepositories
{
    public class EFUserRepository : IUserRepository
    {
        public void SaveUser(User user)
        {
            throw new NotImplementedException();
        }

        public User TryFindByLogin(string login)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Guid> LoadAllUsersIds()
        {
            throw new NotImplementedException();
        }
    }
}