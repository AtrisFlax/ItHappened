using System.Collections.Generic;

namespace ItHappened.Domain.Statistics
{
    public interface IMultipleTrackersFactProvider
    {
        void Add(IMultipleTrackersStatisticsCalculator statisticsCalculator);

        IReadOnlyCollection<IMultipleTrackerTrackerFact> GetFacts(
            IReadOnlyCollection<TrackerWithItsEvents> trackerWithItsEvents);
    }
}