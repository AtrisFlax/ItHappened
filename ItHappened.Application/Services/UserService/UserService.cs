using System;
using System.Net;
using ItHappened.Api.Authentication;
using ItHappened.Application.Errors;
using ItHappened.Domain;
using ItHappened.Infrastructure;

namespace ItHappened.Application.Services.UserService
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IPasswordHasher _passwordHasher;
        private readonly IJwtIssuer _jwtIssuer;


        public UserService(IUserRepository userRepository, IPasswordHasher passwordHasher, IJwtIssuer jwtIssuer)
        {
            _userRepository = userRepository;
            _passwordHasher = passwordHasher;
            _jwtIssuer = jwtIssuer;
        }

        public User Register(string loginName, string password)
        {
            var user = _userRepository.TryFindByLogin(loginName);
            if (user != null)
                throw new RestException(HttpStatusCode.BadRequest, new { Username = "Username already exists" });
            var (hashedPassword, salt) = _passwordHasher.HashWithRandomSalt(password);
            user = new User(Guid.NewGuid(), loginName, new Password(hashedPassword, salt));
            _userRepository.SaveUser(user);
            return user;
        }
        
        public Token Authenticate(string loginName, string password)
        {
            var user = _userRepository.TryFindByLogin(loginName);
            if (user == null)
                throw new RestException(HttpStatusCode.BadRequest, new { User = "User with provided credentials not found" });
            
            var passwordHashedWithSalt = _passwordHasher.HashWithSalt(password, user.Password.Salt);
            if (passwordHashedWithSalt != user.Password.Hash)
                throw new RestException(HttpStatusCode.NotFound, new { User = "User with provided credentials not found", });

            var token = new Token(_jwtIssuer.GenerateToken(user));
            return token;
        }
    }
}