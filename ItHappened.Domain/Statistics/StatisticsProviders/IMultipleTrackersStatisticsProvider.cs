using System.Collections.Generic;

namespace ItHappened.Domain.Statistics
{
    public interface IMultipleTrackersStatisticsProvider
    {
        void Add(IMultipleTrackersStatisticsCalculator calculator);

        IReadOnlyCollection<IStatisticsFact> GetFacts(
            IEnumerable<EventTracker> eventTrackers);
    }
}