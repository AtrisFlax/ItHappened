using System;
using ItHappend.Domain;

namespace ItHappend.Application
{
    interface IUserService
    {
        (Guid id, UserServiceStatusCodes status) RegisterUser(string login, string password, IPasswordHasher securePasswordHasher, IUserRepository userRepository);

        (UserInfo userInfo, UserServiceStatusCodes status) AuthenticateUser(string login, string password, IUserRepository userRepository,
            IEventTrackerRepository eventTrackerRepository, IPasswordHasher securePasswordHasher);
    }
}