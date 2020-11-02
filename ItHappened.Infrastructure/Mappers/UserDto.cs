using System;

namespace ItHappened.Infrastructure.Mappers
{
    public class UserDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string PasswordHash { get; set; }
    }
}