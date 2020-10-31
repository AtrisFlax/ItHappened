using System;

namespace ItHappened.Infrastructure.Mappers
{
    public class UserDB
    {
        public Guid Id { get; }
        public string Name { get; }
        public string Hash { get; }
        public byte[] Salt { get; }
    }
}