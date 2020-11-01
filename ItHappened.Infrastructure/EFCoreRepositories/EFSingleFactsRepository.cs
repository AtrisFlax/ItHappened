using System;
using System.Collections.Generic;
using ItHappened.Domain;
using ItHappened.Domain.Statistics;

namespace ItHappened.Infrastructure.EFCoreRepositories
{
    public class EFSingleFactsRepository : ISingleFactsRepository
    {
        private readonly EFFactsRepository _factsRepository;

        public EFSingleFactsRepository(EFFactsRepository factsRepository)
        {
            _factsRepository = factsRepository;
        }

        public IReadOnlyCollection<ISingleTrackerFact> LoadTrackerSpecificFacts(Guid trackerId)
        {
            return _factsRepository.LoadTrackerSpecificFacts(trackerId);
        }

        public void UpdateTrackerSpecificFacts(Guid trackerId, IReadOnlyCollection<ISingleTrackerFact> facts)
        {
            _factsRepository.LoadTrackerSpecificFacts(trackerId, facts);
        }

        public bool IsContainFactForTracker(Guid trackerId)
        {
            return _factsRepository.IsContainFactForTracker(trackerId);
        }
    }
}