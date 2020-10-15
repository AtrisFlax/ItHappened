using System.Collections.Generic;

namespace ItHappend.Domain.Calculators
{
    public interface IMultipleTrackersStatisticsCalculator<TFact>
    {
        TFact Calculate(IEnumerable<EventTracker> eventTrackers);
    }
}