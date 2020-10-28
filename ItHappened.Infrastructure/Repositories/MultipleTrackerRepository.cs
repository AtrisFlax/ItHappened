using System;
using System.Collections.Generic;
using System.Linq;
using ItHappened.Domain;
using ItHappened.Domain.Statistics;

namespace ItHappened.Infrastructure.Repositories
{
    public class MultipleTrackerRepository : IMultipleTrackerRepository
    {
        private readonly Dictionary<Guid, IEnumerable<IMultipleTrackersFact>> _generalFacts = new Dictionary<Guid, IEnumerable<IMultipleTrackersFact>>();
        public IReadOnlyCollection<IMultipleTrackersFact> LoadUserGeneralFacts(Guid userId)
        {
            return _generalFacts[userId].ToList();
        }

        public void UpdateUserGeneralFacts(IReadOnlyCollection<IMultipleTrackersFact> facts, Guid userId)
        {
            _generalFacts[userId] = facts;
        }
    }
}