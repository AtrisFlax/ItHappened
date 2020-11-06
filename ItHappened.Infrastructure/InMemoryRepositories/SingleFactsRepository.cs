using System;
using System.Collections.Generic;
using System.Linq;
using ItHappened.Domain;
using ItHappened.Domain.Statistics;

namespace ItHappened.Infrastructure.InMemoryRepositories
{
    public class SingleFactsRepository : ISingleFactsRepository
    {
        private readonly Dictionary<Guid, IEnumerable<ISingleTrackerFact>> _specificFacts = new Dictionary<Guid, IEnumerable<ISingleTrackerFact>>();
        public IReadOnlyCollection<ISingleTrackerFact> ReadTrackerSpecificFacts(Guid userId, Guid trackerId)
        {
            return _specificFacts[trackerId].ToList();
        }

        public void CreateTrackerSpecificFacts(Guid trackerId,
            Guid guid,
            IReadOnlyCollection<ISingleTrackerFact> updatedFacts)
        {
            _specificFacts[trackerId] = updatedFacts;
        }

        public bool IsContainFactForTracker(Guid trackerId, Guid userId)
        {
            return _specificFacts.ContainsKey(trackerId);
        }
    }
}