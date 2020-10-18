using System.Collections.Generic;

namespace ItHappened.Domain.Statistics
{
    public interface ISingleTrackerStatisticsProvider
    {
        void Add(ISingleTrackerStatisticsCalculator calculator);
        IReadOnlyCollection<ISingleTrackerStatisticsFact> GetFacts(EventTracker eventTracker);
    }
}