using System;
using ItHappend.Domain;
using Status = ItHappend.Application.UserServiceStatusCodes;

namespace ItHappend.Application
{
    class UserService : IUserService
    {
        public (Guid id, Status status) RegisterUser(string login, string password,
            IPasswordHasher securePasswordHasher,
            IUserRepository userRepository)
        {
            var userInfo = userRepository.TryLoadUserAuthInfo(login);
            if (userInfo != null)
            {
                return (Guid.Empty, Status.UserWithSuchLoginAlreadyExist);
            }
            var user = CreateUser(login, password, securePasswordHasher);
            userRepository.TrySaveUser(user);
            return (user.Guid, Status.Ok);
        }

        public (UserInfo userInfo, Status status) AuthenticateUser(string login, string password,
            IUserRepository userRepository,
            IEventTrackerRepository eventTrackerRepository, IPasswordHasher securePasswordHasher)
        {
            var userAuthInfo = userRepository.TryLoadUserAuthInfo(login);
            if (!securePasswordHasher.Verify(password, userAuthInfo.PasswordHash))
            {
                return (null, Status.WrongPassword);
            }

            var userEventTrackers = eventTrackerRepository.LoadUserTrackers(userAuthInfo.userId);
            return (new UserInfo(userEventTrackers, userAuthInfo.SubscriptionType), Status.Ok);
        }

        private static UserAuthInfo CreateUser(string login, string password, IPasswordHasher securePasswordHasher)
        {
            var userId = Guid.NewGuid();
            var passwordHash = securePasswordHasher.Hash(password);
            return new UserAuthInfo(userId, login, passwordHash, DateTimeOffset.UtcNow);
        }
    }
}