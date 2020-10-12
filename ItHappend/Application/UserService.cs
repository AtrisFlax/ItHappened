using System;
using ItHappend.Domain;

namespace ItHappend.Infrastructure
{
    class UserService : IUserService
    {
        //Переименовать в RegisterUser
        //Следуя стилю Виталика я бы возвращал Guid
        public UserAuthInfo RegistrateUser(string login, string password, IPasswordHasher securePasswordHasher,
            IUserRepository userRepository)
        {
            var user = CreateUser(login, password, securePasswordHasher);
            userRepository.AddUser(user);
            return user;
        }
        //переименовать AuthenticateUser
        //Тут тоже мне кажется достаточно Guid возвращать, а те кто его получит
        //выгрузят все данные из eventTrackerRepository
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