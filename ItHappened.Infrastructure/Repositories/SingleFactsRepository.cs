using System;
using System.Collections.Generic;
using System.Linq;
using ItHappened.Domain;
using ItHappened.Domain.Statistics;

namespace ItHappened.Infrastructure.Repositories
{
    public class SingleFactsRepository : ISingleFactsRepository
    {
        private readonly Dictionary<Guid, IEnumerable<ISingleTrackerFact>> _specificFacts = new Dictionary<Guid, IEnumerable<ISingleTrackerFact>>();
        public IReadOnlyCollection<ISingleTrackerFact> LoadTrackerSpecificFacts(Guid trackerId)
        {
            return _specificFacts[trackerId].ToList();
        }

        public void UpdateTrackerSpecificFacts(Guid trackerId, IReadOnlyCollection<ISingleTrackerFact> updatedFacts)
        {
            _specificFacts[trackerId] = updatedFacts;
        }
    }
}