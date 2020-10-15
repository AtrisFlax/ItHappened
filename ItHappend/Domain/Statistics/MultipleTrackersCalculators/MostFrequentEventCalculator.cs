using System;
using System.Collections.Generic;

namespace ItHappend.Domain.Statistics.MultipleTrackersCalculators
{
    public class MostFrequentEventCalculator : IMultipleTrackersStatisticsCalculator<MostFrequentEvent>
    {
        public MostFrequentEvent Calculate(IEnumerable<EventTracker> eventTrackers)
        {
            throw new NotImplementedException();
        }
    }
}