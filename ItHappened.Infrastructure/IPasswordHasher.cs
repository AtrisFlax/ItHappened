using System;
using System.Security.Cryptography;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;

namespace ItHappened.Infrastructure
{
    public interface IPasswordHasher
    {
        (string hashedPassword, byte[] salt) HashWithRandomSalt(string password);
        string HashWithSalt(string password, byte[] salt);

    }

    public class PasswordHasher : IPasswordHasher
    {
        public (string, byte[]) HashWithRandomSalt(string password)
        {
            // generate a 128-bit salt using a secure PRNG
            byte[] salt = new byte[128 / 8];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(salt);
            }
            
            // derive a 256-bit subkey (use HMACSHA1 with 10,000 iterations)
            var hashedPassword = HashWithSalt(password, salt);
            return (hashedPassword, salt);
        }

        public string HashWithSalt(string password, byte[] salt)
        { 
            // derive a 256-bit subkey (use HMACSHA1 with 10,000 iterations)
            var hashedPassword =  Convert.ToBase64String(KeyDerivation.Pbkdf2(
                password: password,
                salt: salt,
                prf: KeyDerivationPrf.HMACSHA1,
                iterationCount: 10000,
                numBytesRequested: 256 / 8));
            return hashedPassword;
        }
    }
}