using System;
using System.Net;
using System.Reflection.Metadata.Ecma335;

namespace ItHappend
{
    public class UserAuthentication 
    {

        static User Authentication(string login, string password, IUserRepository userRepository,
            IEventTrackerRepository eventTrackerRepository, IPasswordHasher securePasswordHasher)
        {
            var (userId, userPasswordHash) = userRepository.GetAuthInfo(login);
            if (securePasswordHasher.Verify(password, userPasswordHash))
            {
                var userInfo = eventTrackerRepository.GetUserInfo(userId);
                return new User(userInfo.EventTrackers, userInfo.SubscriptionType);
            }
            else
            {
                return null;
            }
        }
    }
}