using System.Collections.Generic;
using LanguageExt;

namespace ItHappend.Domain.Statistics.MultipleTrackersCalculators
{
    public interface IMultipleTrackersStatisticsCalculator<TFact>
    {
        Option<TFact> Calculate(IEnumerable<EventTracker> eventTrackers);
    }
}