using System;
using System.Collections.Generic;
using ItHappened.Domain;
using ItHappened.Domain.Statistics;

namespace ItHappened.Infrastructure.EFCoreRepositories
{
    // ReSharper disable once InconsistentNaming
    public class EFMultipleFactsRepository : IMultipleFactsRepository
    {
        
        private readonly EFFactsRepository _factsRepository;

        public EFMultipleFactsRepository(EFFactsRepository factsRepository)
        {
            _factsRepository = factsRepository;
        }
        public IReadOnlyCollection<IMultipleTrackersFact> LoadUserGeneralFacts(Guid userId)
        {
            return _factsRepository.LoadUserGeneralFacts(userId);
        }

        public void UpdateUserGeneralFacts(Guid userId, IReadOnlyCollection<IMultipleTrackersFact> facts)
        {
            _factsRepository.LoadTrackerSpecificFacts(userId, facts);
        }

        public bool IsContainFactsForUser(Guid userId)
        {
            return _factsRepository.IsContainFactForTracker(userId);
        }
    }
}