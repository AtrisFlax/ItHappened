using System;
using System.Security.Cryptography;

namespace ItHappend.Infrastructure
{
    class PasswordHasher : IPasswordHasher
    {
        private const int SaltSize = 16;
        private const int HashSize = 20;
        private const int HashIterationsNumber = 10000;
        public string Hash(string password)
        {
            var saltBytes = GenerateSalt();
            var passwordHashBytes = HashPasswordUsingSalt(password, saltBytes);
            var passwordHash = CombinePasswordWithSaltHashes(passwordHashBytes, saltBytes);
            return Convert.ToBase64String(passwordHash);
        }

        public bool Verify(string password, string hashedPassword)
        {
            var hashBytes = Convert.FromBase64String(hashedPassword);
            var salt = new byte[SaltSize];
            Array.Copy(hashBytes, 0, salt, 0, SaltSize);
            var pbkdf2Cypher = new Rfc2898DeriveBytes(password, salt, HashIterationsNumber);
            var hash = pbkdf2Cypher.GetBytes(HashSize);
            for (var i = 0; i < HashSize; i++)
            {
                if (hashBytes[i + SaltSize] != hash[i])
                    return false;
            }

            return true;
        } 
        
        private byte[] GenerateSalt()
        {
            byte[] salt;
            new RNGCryptoServiceProvider().GetBytes(salt = new byte[SaltSize]);
            return salt;
        }

        private byte[] HashPasswordUsingSalt(string password, byte[] saltBytes)
        {
            var passwordBytes = Convert.FromBase64String(password);
            var pbkdf2Cypher = new Rfc2898DeriveBytes(passwordBytes, saltBytes, HashIterationsNumber);
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