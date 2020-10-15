using System.Collections.Generic;

namespace ItHappend.Domain.Statistics.MultipleTrackersCalculators
{
    public interface IMultipleTrackersStatisticsCalculator<TFact>
    {
        TFact Calculate(IEnumerable<EventTracker> eventTrackers);
    }
}