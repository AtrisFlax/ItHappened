using System.Collections.Generic;
using ItHappened.Domain.Statistics.MultipleTrackersStatisticsFacts;
using LanguageExt;

namespace ItHappened.Domain.Statistics.MultipleTrackersCalculators
{
    public interface IMultipleTrackersStatisticsCalculator
    {
        Option<IMultipleTrackersStatisticsFact> Calculate(IEnumerable<EventTracker> eventTrackers);
    }
}