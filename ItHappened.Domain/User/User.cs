using System;

namespace ItHappened.Domain
{
    public class User
    {
        public Guid Id { get; private set;}
        public string Name { get; private set;}
        public string PasswordHash { get; private set;}

        public User(Guid id, string name, string passwordHash)
        {
            Id = id;
            Name = name;
            PasswordHash = passwordHash;
        }
    }
}