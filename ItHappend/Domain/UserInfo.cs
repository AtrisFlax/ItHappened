using System.Collections.Generic;
using ItHappend.Domain;

namespace ItHappend
{
    class UserInfo
    {
        public IList<EventTracker> EventTrackers { get; }
        public SubscriptionType SubscriptionType { get; }
    }
}