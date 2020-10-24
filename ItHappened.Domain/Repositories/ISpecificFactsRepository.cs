using System;
using System.Collections.Generic;
using ItHappened.Domain.Statistics;

namespace ItHappened.Domain
{
    public interface ISpecificFactsRepository
    {
        IReadOnlyCollection<ISpecificFact> LoadTrackerSpecificFacts(Guid trackerId);
        void UpdateTrackerSpecificFacts(Guid guid, IReadOnlyCollection<ISpecificFact> trackerId);
    }
}