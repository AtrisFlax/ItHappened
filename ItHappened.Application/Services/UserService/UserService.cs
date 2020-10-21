using System;
using ItHappened.Domain;
using ItHappened.Infrastructure;

namespace ItHappened.Application.Services.UserService
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IPasswordHasher _passwordHasher;
        public UserService(IUserRepository userRepository, IPasswordHasher passwordHasher)
        {
            _userRepository = userRepository;
            _passwordHasher = passwordHasher;
        }

        public User Register(string name, string password)
        {
            var (hashedPassword, salt) = _passwordHasher.HashWithRandomSalt(password);
            var user = new User(Guid.NewGuid(), name, new Password(hashedPassword, salt));
            _userRepository.SaveUser(user);
            return user;
        }

        public User TryFindByLogin(string login)
        {
            var user = _userRepository.TryFindByLogin(login);
            return user;
        }
    }
}