using System;
using Hangfire;
using ItHappened.Application.Authentication;
using ItHappened.Application.Errors;
using ItHappened.Domain;
using ItHappened.Domain.Statistics;
using Microsoft.Extensions.Configuration;

namespace ItHappened.Application.Services.UserService
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IPasswordHasher _passwordHasher;
        private readonly IJwtIssuer _jwtIssuer;
        private readonly IBackgroundStatisticGenerator _backgroundStatisticGenerator;
        private readonly IConfiguration _configuration;

        public UserService(IUserRepository userRepository,
            IPasswordHasher passwordHasher,
            IJwtIssuer jwtIssuer,
            IBackgroundStatisticGenerator backgroundStatisticGenerator,
            IConfiguration configuration)
        {
            _userRepository = userRepository;
            _passwordHasher = passwordHasher;
            _jwtIssuer = jwtIssuer;
            _backgroundStatisticGenerator = backgroundStatisticGenerator;
            _configuration = configuration;
        }

        public UserWithToken Register(string loginName, string password)
        {
            if (_userRepository.LoadUserByLogin(loginName).IsSome)
                throw new UsernameAlreadyInUseException(loginName);
            
            var hashedPassword = _passwordHasher.Hash(password);    
            var user = new User(Guid.NewGuid(), loginName, hashedPassword);
            _userRepository.CreateUser(user);
            RecurringJob.AddOrUpdate($"{user.Id}", () =>
                    _backgroundStatisticGenerator.UpdateUserFacts(user.Id),
                _configuration.GetValue<string>("CronCronRecalculateStatisticPeriod"));
            return new UserWithToken(user, _jwtIssuer.GenerateToken(user));
        }

        public UserWithToken Authenticate(string loginName, string password)
        {
            return _userRepository
                .LoadUserByLogin(loginName)
                .Match((user) =>
            {
                if (!_passwordHasher.Verify(password, user.PasswordHash))
                    throw new UserNotFoundException(loginName, password);
                return new UserWithToken(user, _jwtIssuer.GenerateToken(user));
            }, () => throw new UserNotFoundException(loginName, password));
        }
    }
}