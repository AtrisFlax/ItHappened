using System;
using System.Collections.Generic;
using ItHappened.Domain.Statistics;

namespace ItHappened.Domain
{
    public interface  ISingleTrackerFactsRepository
    {
        IReadOnlyCollection<ISingleTrackerFact> LoadTrackerSpecificFacts(Guid trackerId);
        void UpdateTrackerSpecificFacts(Guid guid, IReadOnlyCollection<ISingleTrackerFact> trackerId);
    }
}