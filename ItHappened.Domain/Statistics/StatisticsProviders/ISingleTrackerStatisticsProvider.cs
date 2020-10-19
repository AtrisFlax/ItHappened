using System.Collections.Generic;

namespace ItHappened.Domain.Statistics
{
    public interface ISingleTrackerStatisticsProvider
    {
        void Add(ISingleTrackerStatisticsCalculator calculator);
        IReadOnlyCollection<IStatisticsFact> GetFacts(EventTracker eventTracker);
    }
}