using System;
using System.Collections.Generic;
using System.Linq;
using ItHappened.Domain;

namespace ItHappened.Infrastructure.EFCoreRepositories
{
    public class EfUserRepository : IUserRepository
    {
        private readonly ItHappenedDbContext _context;

        public EfUserRepository(ItHappenedDbContext context)
        {
            _context = context;
        }

        public void CreateUser(User user)
        {
            _context.Users.Add(user);
        }

        public User TryFindByLogin(string login)
        {
            return _context.Users.FirstOrDefault(user => user.Name == login);
        }

        public IEnumerable<Guid> LoadAllUsersIds()
        {
            throw new NotImplementedException();
        }
    }
}