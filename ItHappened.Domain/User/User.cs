using System;

namespace ItHappened.Domain
{
    public class User
    {
        public User(Guid id, string name, DateTimeOffset dateTimeOffset)
        {
            Id = id;
            Name = name;
            DateTimeOffset = dateTimeOffset;
        }

        public Guid Id { get; }
        public string Name { get; }
        public DateTimeOffset DateTimeOffset { get; }
    }
}