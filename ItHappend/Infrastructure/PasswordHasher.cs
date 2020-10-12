namespace ItHappend
{
    class PasswordHasher : IPasswordHasher
    {
        public string Hash(string password)
        {
            throw new System.NotImplementedException();
        }

        public bool Verify(string password, string hashedPassword)
        {
            throw new System.NotImplementedException();
        }
    }
}