using System.Collections.Generic;

namespace ItHappened.Domain.Statistics
{
    public interface ISingleTrackerStatisticsProvider
    {
        void Add(ISpecificCalculator calculator);
        IReadOnlyCollection<IFact> GetFacts(EventTracker eventTracker);
    }
}