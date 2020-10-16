using ItHappened.Infrastructure;
using NUnit.Framework;

namespace ItHappened.UnitTests
{
    public class PasswordHasherTests
    {
        [Test]
        public void VerifyCorrectPassword_VerificationPassed()
        {
            var passwordHasher = new PasswordHasher();
            const string password = "abc123_-";
            var hashedPassword = passwordHasher.Hash(password);
            const bool expected = true;

            var actual = passwordHasher.Verify(password, hashedPassword);
            
            Assert.AreEqual(expected, actual);
        }
        
        [Test]
        public void VerifyWrongPassword_VerificationFailed()
        {
            var passwordHasher = new PasswordHasher();
            const string password = "-abc123_";
            const string wrongPassword = "abc123_";
            var hashedPassword = passwordHasher.Hash(password);
            const bool expected = false;

            var actual = passwordHasher.Verify(wrongPassword, hashedPassword);
            
            Assert.AreEqual(expected, actual);
        }
    }
}