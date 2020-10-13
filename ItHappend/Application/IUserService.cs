using ItHappend.Domain;

namespace ItHappend.Application
{
    interface IUserService
    {
        UserAuthInfo RegisterUser(string login, string password, IPasswordHasher securePasswordHasher, IUserRepository userRepository);

        UserInfo AuthenticateUser(string login, string password, IUserRepository userRepository,
            IEventTrackerRepository eventTrackerRepository, IPasswordHasher securePasswordHasher);
    }
}