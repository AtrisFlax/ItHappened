using System;
using ItHappend.Domain;
using Status = ItHappend.Application.UserServiceStatusCodes;

namespace ItHappend.Application
{
    public class UserService : IUserService
    {
        private IUserRepository _userRepository;
        private IPasswordHasher _securePasswordHasher;
        private IEventTrackerRepository _eventTrackerRepository;
        public UserService(IUserRepository userRepository, IPasswordHasher securePasswordHasher, IEventTrackerRepository eventTrackerRepository)
        {
            _userRepository = userRepository;
            _securePasswordHasher = securePasswordHasher;
            _eventTrackerRepository = eventTrackerRepository;
        }
        public (Guid id, Status status) RegisterUser(string login, string password)
        {
            var userInfo = _userRepository.TryLoadUserAuthInfo(login);
            if (userInfo != null)
            {
                return (Guid.Empty, Status.UserWithSuchLoginAlreadyExist);
            }
            var user = CreateUser(login, password);
            _userRepository.SaveUser(user);
            return (user.Guid, Status.Ok);
        }

        public (UserInfo userInfo, Status status) AuthenticateUser(string login, string password)
        {
            var userAuthInfo = _userRepository.TryLoadUserAuthInfo(login);
            if (!_securePasswordHasher.Verify(password, userAuthInfo.PasswordHash))
            {
                return (null, Status.WrongPassword);
            }

            var userEventTrackers = _eventTrackerRepository.LoadUserTrackers(userAuthInfo.userId);
            return (new UserInfo(userEventTrackers, userAuthInfo.SubscriptionType), Status.Ok);
        }

        private UserAuthInfo CreateUser(string login, string password)
        {
            var userId = Guid.NewGuid();
            var passwordHash = _securePasswordHasher.Hash(password);
            return new UserAuthInfo(userId, login, passwordHash, DateTimeOffset.UtcNow);
        }
    }
}