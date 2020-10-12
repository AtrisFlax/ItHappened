using System.Collections.Generic;

namespace ItHappend
{
    public class User
    {
        public IList<EventTracker> EventTrackers { get; }

        public SubscriptionType SubscriptionType { get; }

        public User(IList<EventTracker> eventTrackers, SubscriptionType subscriptionType)
        {
            EventTrackers = eventTrackers;
            SubscriptionType = subscriptionType;
        }
    }
}