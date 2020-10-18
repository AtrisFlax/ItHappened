using System.Collections.Generic;
using LanguageExt;

namespace ItHappened.Domain.Statistics
{
    public interface IMultipleTrackersStatisticsCalculator
    {
        Option<IMultipleTrackersStatisticsFact> Calculate(IEnumerable<EventTracker> eventTrackers);
    }
}