using System.Collections.Generic;

namespace ItHappened.Domain.Statistics
{
    public interface IGeneralFactProvider
    {
        void Add(IGeneralCalculator calculator);

        IReadOnlyCollection<IGeneralFact> GetFacts(IEnumerable<EventTracker> eventTrackers);
    }
}