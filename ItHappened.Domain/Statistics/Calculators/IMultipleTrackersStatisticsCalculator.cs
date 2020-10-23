using System.Collections.Generic;
using LanguageExt;

namespace ItHappened.Domain.Statistics
{
    public interface IMultipleTrackersStatisticsCalculator
    {
        Option<IMultipleTrackerTrackerFact> Calculate(IReadOnlyCollection<TrackerWithItsEvents> trackerWithItsEvents);
    }
}