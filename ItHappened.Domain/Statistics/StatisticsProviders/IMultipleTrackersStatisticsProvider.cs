using System.Collections.Generic;

namespace ItHappened.Domain.Statistics
{
    public interface IMultipleTrackersStatisticsProvider
    {
        void Add(IGeneralCalculator calculator);

        IReadOnlyCollection<IFact> GetFacts(
            IEnumerable<EventTracker> eventTrackers);
    }
}