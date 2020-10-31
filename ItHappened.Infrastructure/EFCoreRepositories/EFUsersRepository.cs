using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using ItHappened.Domain;
using ItHappened.Infrastructure.Mappers;

namespace ItHappened.Infrastructure.EFCoreRepositories
{
    public class EFUserRepository : IUserRepository
    {
        private readonly IMapper _mapper;
        private readonly ItHappenedDbContext _context;

        public EFUserRepository(ItHappenedDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public void CreateUser(User user)
        {
            var userDb = _mapper.Map<UserDB>(user);
            _context.Users.Add(userDb);
        }

        public User TryFindByLogin(string login)
        {
            var userDB = _context.Users.First(user => user.Name == login);
            return _mapper.Map<User>(userDB);
        }

        public IEnumerable<Guid> LoadAllUsersIds()
        {
            throw new NotImplementedException();
        }
    }
}