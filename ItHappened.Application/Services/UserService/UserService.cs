using System;
using System.Net;
using Hangfire;
using ItHappened.Application.Authentication;
using ItHappened.Application.Errors;
using ItHappened.Domain;
using ItHappened.Domain.Statistics;

namespace ItHappened.Application.Services.UserService
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IPasswordHasher _passwordHasher;
        private readonly IJwtIssuer _jwtIssuer;
        private readonly IBackgroundStatisticGenerator _backgroundStatisticGenerator;

        public UserService(IUserRepository userRepository, 
            IPasswordHasher passwordHasher, 
            IJwtIssuer jwtIssuer,
            IBackgroundStatisticGenerator backgroundStatisticGenerator)
        {
            _userRepository = userRepository;
            _passwordHasher = passwordHasher;
            _jwtIssuer = jwtIssuer;
            _backgroundStatisticGenerator = backgroundStatisticGenerator;
        }

        public UserWithToken Register(string loginName, string password)
        {
            var user = _userRepository.TryFindByLogin(loginName);
            if (user != null)
                throw new RestException(HttpStatusCode.BadRequest, new { Username = "Username already exists" });
            var (hashedPassword, salt) = _passwordHasher.HashWithRandomSalt(password);
            user = new User(Guid.NewGuid(), loginName, new Password(hashedPassword, salt));
            _userRepository.SaveUser(user);
            RecurringJob.AddOrUpdate($"{user.Id}",() => 
                _backgroundStatisticGenerator.UpdateUserFacts(user.Id), Cron.Hourly);
            return new UserWithToken(user, _jwtIssuer.GenerateToken(user));
        }
        
        public UserWithToken Authenticate(string loginName, string password)
        {
            var user = _userRepository.TryFindByLogin(loginName);
            if (user == null)
                throw new RestException(HttpStatusCode.BadRequest, new { User = "User with provided credentials not found" });
            
            var passwordHashedWithSalt = _passwordHasher.HashWithSalt(password, user.Password.Salt);
            if (passwordHashedWithSalt != user.Password.Hash)
                throw new RestException(HttpStatusCode.NotFound, new { User = "User with provided credentials not found", });

            return new UserWithToken(user, _jwtIssuer.GenerateToken(user));
        }
    }
}