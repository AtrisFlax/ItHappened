using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using ItHappened.Application.Errors;
using ItHappened.Domain;
using ItHappened.Infrastructure.Mappers;
using LanguageExt;

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

        public Option<User> LoadUserByLogin(string login)
        {
            var dtoUser = _context.Users.Find(user => user.Name == login);
            return dtoUser.Match(dto => _mapper.Map<User>(dto), Option<User>.None);
        }
        
        public IEnumerable<Guid> LoadAllUsersIds()
        {
            throw new NotImplementedException();
        }
    }
}