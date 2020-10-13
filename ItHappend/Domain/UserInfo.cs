using System.Collections.Generic;

namespace ItHappend.Domain
{
    public class UserInfo
    {
        public IList<EventTracker> EventTrackers { get; }
        public SubscriptionType SubscriptionType { get; }
        public UserInfo(IList<EventTracker> eventTrackers, SubscriptionType subscriptionType)
        {
            EventTrackers = eventTrackers;
            SubscriptionType = subscriptionType;
        }
    }
}