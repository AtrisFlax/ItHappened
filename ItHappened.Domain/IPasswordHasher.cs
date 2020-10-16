﻿namespace ItHappened.Domain
{
    public interface IPasswordHasher
    {
        string Hash(string password);
        bool Verify(string passwordToVerify, string hashedPassword);
    }
}