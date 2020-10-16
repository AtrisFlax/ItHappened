using System;
using System.Security.Cryptography;
using ItHappened.Domain;

namespace ItHappened.Infrastructure
{
    public class PasswordHasher : IPasswordHasher
    {
        private const int SaltSize = 16;
        private const int HashSize = 20;
        private const int HashIterationsNumber = 10000;
        public string Hash(string password)
        {
            var saltBytes = GenerateSalt();
            var passwordHashBytes = HashPasswordUsingSalt(password, saltBytes);
            var completePasswordHashBytes = CombinePasswordWithSaltHashes(passwordHashBytes, saltBytes);
            return Convert.ToBase64String(completePasswordHashBytes);
        }

        public bool Verify(string passwordToVerify, string hashedPassword)
        {
            var hashBytes = Convert.FromBase64String(hashedPassword);
            var salt = GetSaltFromHash(hashBytes);
            var passwordToVerifyWithSalt = HashPasswordUsingSalt(passwordToVerify, salt);
            for (var i = 0; i < HashSize; i++)
            {
                if (hashBytes[i + SaltSize] != passwordToVerifyWithSalt[i])
                    return false;
            }

            return true;
        }

        private static byte[] GetSaltFromHash(byte[] hashBytes)
        {
            var salt = new byte[SaltSize];
            Array.Copy(hashBytes, 0, salt, 0, SaltSize);
            return salt;
        }

        private byte[] GenerateSalt()
        {
            byte[] salt;
            new RNGCryptoServiceProvider().GetBytes(salt = new byte[SaltSize]);
            return salt;
        }

        private byte[] HashPasswordUsingSalt(string password, byte[] saltBytes)
        {
            var pbkdf2Cypher = new Rfc2898DeriveBytes(password, saltBytes, HashIterationsNumber);
            var passwordSaltedHash = pbkdf2Cypher.GetBytes(HashSize);
            return passwordSaltedHash;
        }

        private byte[] CombinePasswordWithSaltHashes(byte[] passwordSaltHash, byte[] salt)
        {
            var hashBytes = new byte[SaltSize + HashSize];
            Array.Copy(salt, 0, hashBytes, 0, SaltSize);
            Array.Copy(passwordSaltHash, 0, hashBytes, SaltSize, HashSize);
            return hashBytes;
        }
    }
}