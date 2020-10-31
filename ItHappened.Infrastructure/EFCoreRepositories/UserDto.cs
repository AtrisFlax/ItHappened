using System;

namespace ItHappened.Infrastructure.EFCoreRepositories
{
    public class UserDto
    {
        public Guid Id { get; set;}
        public string Name { get; set;}
        public string PasswordHash { get; set;}
    }
}