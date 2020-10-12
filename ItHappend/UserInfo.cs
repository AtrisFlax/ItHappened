using System.Collections.Generic;

namespace ItHappend
{
    class UserInfo
    {
        public IList<EventTracker> EventTrackers { get; }
        public SubscriptionType SubscriptionType { get; }
    }
}