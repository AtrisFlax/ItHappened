using System;
using ItHappend.Domain;

namespace ItHappend
{
    interface IUserService
    {
        UserAuthInfo RegistrateUser(string login, string password, IPasswordHasher securePasswordHasher, IUserRepository userRepository);

        User AuntificateUser(string login, string password, IUserRepository userRepository,
            IEventTrackerRepository eventTrackerRepository, IPasswordHasher securePasswordHasher);
    }
}