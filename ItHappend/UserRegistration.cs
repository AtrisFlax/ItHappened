using System;
using System.Security.Cryptography;

namespace ItHappend
{
    public class UserRegistration
    {
        static UserAuthInfo Registrate(string login, string password, IPasswordHasher securePasswordHasher, IUserRepository userRepository)
        {
            if (login == null) throw new ArgumentNullException(nameof(login));
            if (password == null) throw new ArgumentNullException(nameof(password));
            var user = CreateUser(login, password, securePasswordHasher);
            userRepository.AddUser(user);
            return user;
        }

        private static UserAuthInfo CreateUser(string login, string password, IPasswordHasher securePasswordHasher)
        {
            var userId = Guid.NewGuid();
            var passwordHash = securePasswordHasher.Hash(password);
            return new UserAuthInfo(userId, login, passwordHash);
        }
    }
}