using System;
using ItHappend.Domain;

namespace ItHappend
{
    interface IUserService
    {
        //переименовать RegisterUser
        UserAuthInfo RegistrateUser(string login, string password, IPasswordHasher securePasswordHasher, IUserRepository userRepository);
        //переименовать AuthenticateUser
        User AuntificateUser(string login, string password, IUserRepository userRepository,
            IEventTrackerRepository eventTrackerRepository, IPasswordHasher securePasswordHasher);
    }
}