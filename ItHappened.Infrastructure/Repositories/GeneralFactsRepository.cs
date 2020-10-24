using System;
using System.Collections.Generic;
using System.Linq;
using ItHappened.Domain;
using ItHappened.Domain.Statistics;

namespace ItHappened.Infrastructure.Repositories
{
    public class GeneralFactsRepository : IGeneralFactsRepository
    {
        private readonly Dictionary<Guid, IEnumerable<IGeneralFact>> _generalFacts = new Dictionary<Guid, IEnumerable<IGeneralFact>>();
        public IReadOnlyCollection<IGeneralFact> LoadUserGeneralFacts(Guid userId)
        {
            return _generalFacts[userId].ToList();
        }

        public void UpdateUserGeneralFacts(IReadOnlyCollection<IGeneralFact> facts, Guid userId)
        {
            _generalFacts[userId] = facts;
        }
    }
}