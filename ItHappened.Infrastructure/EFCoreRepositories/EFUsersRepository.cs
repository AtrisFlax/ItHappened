using System;
using System.Collections.Generic;
using System.Linq;
using ItHappened.Domain;

namespace ItHappened.Infrastructure.EFCoreRepositories
{
    public class EFUserRepository : IUserRepository
    {
        private readonly ItHappenedDbContext _context;

        public EFUserRepository(ItHappenedDbContext context)
        {
            _context = context;
        }

        public void CreateUser(User user)
        {
            _context.Users.Add(user);
        }

        public User TryFindByLogin(string login)
        {
            return _context.Users.First(user => user.Name == login);
        }

        public IEnumerable<Guid> LoadAllUsersIds()
        {
            throw new NotImplementedException();
        }
    }
}