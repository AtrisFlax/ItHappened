using System;

namespace ItHappened.Domain.User
{
    public class UserAuthInfo
    {
        public Guid Guid { get; }
        public string Login { get; }
        public string PasswordHash { get; }
        public DateTimeOffset DateTimeOffset { get; }

        public UserAuthInfo(Guid guid, string login, string passwordHash, DateTimeOffset dateTimeOffset)
        {
            Guid = guid;
            Login = login;
            PasswordHash = passwordHash;
            DateTimeOffset = dateTimeOffset;
        }

        public Guid userId { get; }

        public SubscriptionType SubscriptionType { get; }
    }
}