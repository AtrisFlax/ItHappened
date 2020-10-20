using System;

namespace ItHappened.Domain
{
    public class User
    {
        public User(Guid id, string name, DateTimeOffset dateTimeOffset, string passwordHash)
        {
            Id = id;
            Name = name;
            PasswordHash = passwordHash;
            DateTimeOffset = dateTimeOffset;
        }

        public Guid Id { get; }
        public string Name { get; }
        public string PasswordHash { get; }
        public DateTimeOffset DateTimeOffset { get; }
    }
}