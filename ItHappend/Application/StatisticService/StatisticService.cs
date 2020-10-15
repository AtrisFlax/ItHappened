using System;
using ItHappend.Domain;
using ItHappend.Domain.Statistics;

namespace ItHappend.StatisticService
{
    public class StatisticService : IMostFrequentEvent, IFixedNEvents
    {
        public (MostFrequentEvent, StatisticServiceStatusCodes) GetMostFrequentEventFact(EventTracker[] eventTrackers)
        {
            return MostFrequentEvent.CreateFactStatistics(eventTrackers);
        }

        public (FixedNEventsFact, StatisticServiceStatusCodes) GetFixedNEventsFact()
        {
            throw new NotImplementedException();
        }
    }
}