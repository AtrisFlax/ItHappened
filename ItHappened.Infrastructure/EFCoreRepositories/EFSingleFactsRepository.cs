using System;
using System.Collections.Generic;
using AutoMapper;
using ItHappened.Domain;
using ItHappened.Domain.Statistics;

namespace ItHappened.Infrastructure.EFCoreRepositories
{
    public class EFSingleFactsRepository : ISingleFactsRepository
    {
        private readonly EFFactsRepository _factsRepository;

        public EFSingleFactsRepository(IMapper mapper, ItHappenedDbContext context)
        {
            _factsRepository = new EFFactsRepository(context, mapper); 
        }

        public IReadOnlyCollection<ISingleTrackerFact> ReadTrackerSpecificFacts(Guid userId, Guid trackerId)
        {
            return _factsRepository.ReadTrackerSpecificFacts(userId, trackerId);
        }

        public void CreateTrackerSpecificFacts(Guid trackerId, Guid userId,
            IReadOnlyCollection<ISingleTrackerFact> facts)
        {
            _factsRepository.CreateTrackerSpecificFacts(userId, trackerId, facts);
        }

        public bool IsContainFactForTracker(Guid trackerId, Guid userId)
        {
            return _factsRepository.IsContainFactForTracker(trackerId, userId);
        }
    }
}