using System.Collections.Generic;

namespace ItHappened.Domain.Statistics
{
    public interface ISingleTrackerFactProvider
    {
        void Add(ISingleTrackerStatisticsCalculator calculator);

        IReadOnlyCollection<ISingleTrackerFact>
            GetFacts(IReadOnlyCollection<Event> events, EventTracker tracker);
    }
}