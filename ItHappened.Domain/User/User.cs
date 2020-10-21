using System;

namespace ItHappened.Domain
{
    public class User
    {
        public User(Guid id, string name, Password password)
        {
            Id = id;
            Name = name;
            Password = password;
        }

        public Guid Id { get; }
        public string Name { get; }
        public Password Password { get; }
    }
}