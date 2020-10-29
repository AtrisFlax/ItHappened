using System;
using System.Collections.Generic;
using ItHappened.Domain;
using ItHappened.Domain.Statistics;

namespace ItHappened.Infrastructure.EFCoreRepositories
{
    public class EFMultipleFactsRepository : IMultipleFactsRepository
    {
        public IReadOnlyCollection<IMultipleTrackersFact> LoadUserGeneralFacts(Guid userId)
        {
            throw new NotImplementedException();
        }

        public void UpdateUserGeneralFacts(Guid userId, IReadOnlyCollection<IMultipleTrackersFact> facts)
        {
            throw new NotImplementedException();
        }

        public bool IsContainFactsForUser(Guid userId)
        {
            throw new NotImplementedException();
        }
    }
}