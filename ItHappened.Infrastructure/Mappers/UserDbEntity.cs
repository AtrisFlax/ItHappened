using System;

namespace ItHappened.Infrastructure.Mappers
{
    public class UserDbEntity
    {
        public Guid Id { get; }
        public string Name { get; }
        public string Hash { get; }
        public byte[] Salt { get; }
    }
}