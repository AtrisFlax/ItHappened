namespace ItHappened.Application.Authentication
{
    public interface IPasswordHasher
    {
        (string hashedPassword, byte[] salt) HashWithRandomSalt(string password);
        string HashWithSalt(string password, byte[] salt);
    }
}