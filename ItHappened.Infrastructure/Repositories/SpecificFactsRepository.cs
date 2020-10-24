using System;
using System.Collections.Generic;
using System.Linq;
using ItHappened.Domain;
using ItHappened.Domain.Statistics;

namespace ItHappened.Infrastructure.Repositories
{
    public class SpecificFactsRepository : ISpecificFactsRepository
    {
        private readonly Dictionary<Guid, IEnumerable<ISpecificFact>> _specificFacts = new Dictionary<Guid, IEnumerable<ISpecificFact>>();
        public IReadOnlyCollection<ISpecificFact> LoadTrackerSpecificFacts(Guid trackerId)
        {
            return _specificFacts[trackerId].ToList();
        }

        public void UpdateTrackerSpecificFacts(Guid trackerId, IReadOnlyCollection<ISpecificFact> updatedFacts)
        {
            _specificFacts[trackerId] = updatedFacts;
        }
    }
}