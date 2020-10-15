using System;
using System.Collections.Generic;
using LanguageExt;

namespace ItHappend.Domain.Statistics.MultipleTrackersCalculators
{
    public class MostFrequentEventCalculator : IMultipleTrackersStatisticsCalculator<MostFrequentEvent>
    {
        public Option<MostFrequentEvent> Calculate(IEnumerable<EventTracker> eventTrackers)
        {
            throw new NotImplementedException();
        }
    }
}