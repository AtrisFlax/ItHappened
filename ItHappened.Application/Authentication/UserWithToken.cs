using ItHappened.Domain;

namespace ItHappened.Application.Authentication
{
    public class UserWithToken
    {
        public UserWithToken(User user, string token)
        {
            User = user;
            Token = token;
        }
        
        public User User { get; }
        public string Token { get; }
    }
}