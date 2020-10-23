using System.Collections.Generic;
using LanguageExt;

namespace ItHappened.Domain.Statistics
{
    public interface IMultipleTrackersStatisticsCalculator
    {
        Option<IMultipleTrackersFact> Calculate(IEnumerable<EventTracker> eventTrackers);
    }
}