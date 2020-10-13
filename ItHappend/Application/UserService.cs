using System;
using ItHappend.Domain;

namespace ItHappend.Application
{
    class UserService : IUserService
    {
        public UserAuthInfo RegisterUser(string login, string password, IPasswordHasher securePasswordHasher,
            IUserRepository userRepository)
        {
            var user = CreateUser(login, password, securePasswordHasher);
            userRepository.SaveUser(user);
            return user;
        }

        public UserInfo AuthenticateUser(string login, string password, IUserRepository userRepository,
            IEventTrackerRepository eventTrackerRepository, IPasswordHasher securePasswordHasher)
        {
            var userAuthInfo = userRepository.LoadUserAuthInfo(login);
            if (!securePasswordHasher.Verify(password, userAuthInfo.PasswordHash))
            {
                return null;
            }

            var userEventTrackers = eventTrackerRepository.LoadUserTrackers(userAuthInfo.userId);
            return new UserInfo(userEventTrackers, userAuthInfo.SubscriptionType);
        }

        private static UserAuthInfo CreateUser(string login, string password, IPasswordHasher securePasswordHasher)
        {
            var userId = Guid.NewGuid();
            var passwordHash = securePasswordHasher.Hash(password);
            return new UserAuthInfo(userId, login, passwordHash, DateTimeOffset.UtcNow);
        }
    }
}