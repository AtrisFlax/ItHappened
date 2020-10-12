using System;

namespace ItHappend.Domain.Exceptions
{
    public class UserNotFoundException : System.Exception
    {
        public UserNotFoundException(Guid userId) : base($"User with id {userId} not found")
        {
        }
        
        public UserNotFoundException(string login) : base($"User with login {login} not found")
        {
        }
    }
}