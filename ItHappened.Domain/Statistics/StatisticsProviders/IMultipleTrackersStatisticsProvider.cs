using System.Collections.Generic;

namespace ItHappened.Domain.Statistics
{
    public interface IMultipleTrackersStatisticsProvider
    {
        void Add(IMultipleTrackersStatisticsCalculator calculator);

        IReadOnlyCollection<IMultipleTrackersStatisticsFact> GetFacts(
            IEnumerable<EventTracker> eventTrackers);
    }
}