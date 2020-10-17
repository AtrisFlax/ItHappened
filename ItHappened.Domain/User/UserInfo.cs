using System.Collections.Generic;

namespace ItHappened.Domain.User
{
    public class UserInfo
    {
        public IList<EventTracker> EventTrackers { get; }
        public SubscriptionType SubscriptionType { get; } //not using now 
        public UserInfo(IList<EventTracker> eventTrackers, SubscriptionType subscriptionType)
        {
            EventTrackers = eventTrackers;
            SubscriptionType = subscriptionType;
        }
    }
}