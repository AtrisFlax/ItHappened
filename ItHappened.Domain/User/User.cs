using System;

namespace ItHappened.Domain
{
    public class User
    {
        public Guid Id { get; }
        public string Name { get; }
        public string PasswordHash { get; }

        public User(Guid id, string name, string passwordHash)
        {
            Id = id;
            Name = name;
            PasswordHash = passwordHash;
        }
    }
}