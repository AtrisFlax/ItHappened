using System;
using ItHappend.Domain;

namespace ItHappend.Application
{
    class UserService : IUserService
    {
        public UserAuthInfo RegistrateUser(string login, string password, IPasswordHasher securePasswordHasher,
            IUserRepository userRepository)
        {
            var user = CreateUser(login, password, securePasswordHasher);
            userRepository.AddUser(user);
            return user;
        }

        public User AuntificateUser(string login, string password, IUserRepository userRepository,
            IEventTrackerRepository eventTrackerRepository, IPasswordHasher securePasswordHasher)
        {
            var userAuthInfo = userRepository.GetAuthInfo(login);
            if (!securePasswordHasher.Verify(password, userAuthInfo.PasswordHash))
            {
                return null;
            }

            var userInfo = eventTrackerRepository.GetUserInfo(userAuthInfo.userId);
            return new User(userInfo.EventTrackers, userInfo.SubscriptionType);
        }

        private static UserAuthInfo CreateUser(string login, string password, IPasswordHasher securePasswordHasher)
        {
            var userId = Guid.NewGuid();
            var passwordHash = securePasswordHasher.Hash(password);
            return new UserAuthInfo(userId, login, passwordHash, DateTimeOffset.UtcNow);
        }
    }
}