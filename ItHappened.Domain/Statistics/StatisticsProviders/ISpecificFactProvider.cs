using System.Collections.Generic;

namespace ItHappened.Domain.Statistics
{
    public interface ISpecificFactProvider
    {
        void Add(ISpecificCalculator calculator);
        IReadOnlyCollection<ISpecificFact> GetFacts(EventTracker eventTracker);
    }
}