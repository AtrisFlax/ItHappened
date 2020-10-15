using System;
using System.Collections.Generic;

namespace ItHappend.Domain.Calculators
{
    public interface IMultipleTrackersStatisticsCalculator<TFact>
    {
        TFact Calculate(IEnumerable<EventTracker> eventTrackers);
    }

    public class MostFrequentEventCalculator : IMultipleTrackersStatisticsCalculator<MostFrequentEvent>
    {
        public MostFrequentEvent Calculate(IEnumerable<EventTracker> eventTrackers)
        {
            throw new NotImplementedException();
        }
    }
}