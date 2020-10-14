using System;
using System.Collections.Generic;
using System.Linq;
using ItHappend.Domain;

namespace ItHappend.Statistic
{
    public class StatisticService
    {
        public TheMostFrequentEvent GetMostFrequentEvent(IList<EventTracker> eventTrackers, Guid initiatorId)
        {
            if (eventTrackers.Any(tracker => tracker.CreatorId != initiatorId)) return null;
            var service = new MostFrequentEventsService(eventTrackers, initiatorId);
            var statisticFact = service.CreateFact();
            return statisticFact;
        }
        
    }
}