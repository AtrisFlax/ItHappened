namespace ItHappened.Domain
{
    public class Password
    {
        public Password(string hash, byte[] salt)
        {
            Hash = hash;
            Salt = salt;
        }
        public string Hash { get; }
        public byte[] Salt { get; }
    }
}