using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using ItHappened.Domain;
using ItHappened.Infrastructure.Dto;

namespace ItHappened.Infrastructure.EFCoreRepositories
{
    // ReSharper disable once InconsistentNaming
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

        public User TryFindByLogin(string login)
        {
            var dtoUser = _context.Users.SingleOrDefault(user => user.Name == login);
            return _mapper.Map<User>(dtoUser);
        }
    }
}