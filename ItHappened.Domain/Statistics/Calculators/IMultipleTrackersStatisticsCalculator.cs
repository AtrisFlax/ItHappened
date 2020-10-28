using System;
using System.Collections.Generic;
using LanguageExt;

namespace ItHappened.Domain.Statistics
{
    public interface IMultipleTrackersStatisticsCalculator
    {
        Option<IMultipleTrackersFact> Calculate(IReadOnlyCollection<TrackerWithItsEvents> trackerWithItsEvents, DateTimeOffset now);
    }
}