using System;
using System.Collections.Generic;
using System.Linq;
using ItHappened.Domain;
using ItHappened.Domain.Statistics;

namespace ItHappened.Infrastructure.Repositories
{
    public class MultipleFactsRepository : IMultipleFactsRepository
    {
        private readonly Dictionary<Guid, IEnumerable<IMultipleTrackersFact>> _generalFacts = new Dictionary<Guid, IEnumerable<IMultipleTrackersFact>>();
        public IReadOnlyCollection<IMultipleTrackersFact> ReadUserGeneralFacts(Guid userId)
        {
            return _generalFacts[userId].ToList();
        }

        public void CreateUserGeneralFacts(Guid userId, IReadOnlyCollection<IMultipleTrackersFact> facts)
        {
            _generalFacts[userId] = facts;
        }

        public bool IsContainFactsForUser(Guid userId)
        {
            return _generalFacts.ContainsKey(userId);
        }
    }
}