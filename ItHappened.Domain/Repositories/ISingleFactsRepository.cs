using System;
using System.Collections.Generic;
using ItHappened.Domain.Statistics;

namespace ItHappened.Domain
{
    public interface ISingleFactsRepository
    {
        IReadOnlyCollection<ISingleTrackerFact> ReadTrackerSpecificFacts(Guid userId, Guid trackerId);
        void CreateTrackerSpecificFacts(Guid trackerId, Guid userId, IReadOnlyCollection<ISingleTrackerFact> facts);
        bool IsContainFactForTracker(Guid trackerId, Guid userId);
    }
}