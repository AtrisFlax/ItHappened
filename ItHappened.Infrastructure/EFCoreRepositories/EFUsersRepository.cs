using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using ItHappened.Domain;

namespace ItHappened.Infrastructure.EFCoreRepositories
{
    public class EfUserRepository : IUserRepository
    {
        private readonly ItHappenedDbContext _context;
        private readonly IMapper _mapper;

        public EfUserRepository(ItHappenedDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public void CreateUser(User user)
        {
            var userDto = _mapper.Map<UserDto>(user);
            _context.Users.Add(userDto);
        }

        public User TryFindByLogin(string login)
        {
            var userDto = _context.Users.FirstOrDefault(user => user.Name == login);
            return _mapper.Map<User>(userDto);
        }

        public IEnumerable<Guid> LoadAllUsersIds()
        {
            throw new NotImplementedException();
        }
    }
}