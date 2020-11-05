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
        private readonly ItHappenedDbContext _context;
        private readonly IMapper _mapper;

        public EFUserRepository(ItHappenedDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public void CreateUser(User user)
        {
            var dtoUser = _mapper.Map<UserDto>(user);
            _context.Users.Add(dtoUser);
        }

        public User LoadUser(string loginName)
        {
            var dtoUser = _context.Users.Find(user => user.Name == loginName);
            return _mapper.Map<User>(dtoUser);    
        }

        public bool HasUserWithLogin(string loginName)
        {
            return _context.Users.Any(o => o.Name == loginName);
        }

        public IEnumerable<Guid> LoadAllUsersIds()
        {
            throw new NotImplementedException();
        }
    }
}