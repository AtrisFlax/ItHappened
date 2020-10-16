using System.Collections.Generic;
using ItHappend.Domain.Statistics.MultipleTrackersStatisticsFacts;
using LanguageExt;

namespace ItHappend.Domain.Statistics.MultipleTrackersCalculators
{
    public interface IMultipleTrackersStatisticsCalculator
    {
        Option<IMultipleTrackersStatisticsFact> Calculate(IEnumerable<EventTracker> eventTrackers);
    }
}