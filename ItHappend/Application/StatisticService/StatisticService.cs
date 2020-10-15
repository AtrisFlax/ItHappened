using System;
using System.Collections.Generic;
using System.Linq;
using ItHappend.Domain;

namespace ItHappend.StatisticService
{
    public class StatisticService : IMostFrequentEvent, IFixedNEvents
    {
        public (MostFrequentEvent, StatisticServiceStatusCodes) GetMostFrequentEventFact(EventTracker[] eventTrackers)
        {
            return MostFrequentEvent.CreateFactStatistics(eventTrackers);
        }

        public IFactStatistics GetFixedNEventsFact()
        {
            throw new NotImplementedException();
        }
    }
}