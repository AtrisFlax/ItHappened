using System;

namespace ItHappend
{
    public class UserAuthInfo
    {
        private readonly Guid _guid;
        private readonly string _login;
        private readonly string _passwordHash;

        public UserAuthInfo(Guid guid, string login, string passwordHash)
        {
            _guid = guid;
            _login = login;
            _passwordHash = passwordHash;
        }

        public Guid userId { get; }

        public SubscriptionType SubscriptionType { get; }
        
    }
}