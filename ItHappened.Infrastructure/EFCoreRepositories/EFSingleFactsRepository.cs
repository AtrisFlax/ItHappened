using System;
using System.Collections.Generic;
using ItHappened.Domain;
using ItHappened.Domain.Statistics;

namespace ItHappened.Infrastructure.EFCoreRepositories
{
    public class EFSingleFactsRepository : ISingleFactsRepository
    {
        public IReadOnlyCollection<ISingleTrackerFact> LoadTrackerSpecificFacts(Guid trackerId)
        {
            throw new NotImplementedException();
        }

        public void UpdateTrackerSpecificFacts(Guid trackerId, IReadOnlyCollection<ISingleTrackerFact> facts)
        {
            throw new NotImplementedException();
        }

        public bool IsContainFactForTracker(Guid trackerId)
        {
            throw new NotImplementedException();
        }
    }
}