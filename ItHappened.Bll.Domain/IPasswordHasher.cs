namespace ItHappened.Bll.Domain
{
    public interface IPasswordHasher
    {
        string Hash(string password);
        bool Verify(string passwordToVerify, string hashedPassword);
    }
}