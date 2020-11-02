using System;
using System.Collections.Generic;
using AutoMapper;
using ItHappened.Domain;
using ItHappened.Domain.Statistics;

namespace ItHappened.Infrastructure.EFCoreRepositories
{
    // ReSharper disable once InconsistentNaming
    public class EFMultipleFactsRepository : IMultipleFactsRepository
    {
        private readonly EFFactsRepository _factsRepository;

        public EFMultipleFactsRepository(IMapper mapper, ItHappenedDbContext context)
        {
            _factsRepository = new EFFactsRepository(context, mapper);
        }
        public IReadOnlyCollection<IMultipleTrackersFact> ReadUserGeneralFacts(Guid userId)
        {
            return _factsRepository.LoadUserGeneralFacts(userId);
        }

        public void CreateUserGeneralFacts(Guid userId, IReadOnlyCollection<IMultipleTrackersFact> facts)
        {
            _factsRepository.CreateTrackerSpecificFacts(userId, facts);
        }

        public bool IsContainFactsForUser(Guid userId)
        {
            return _factsRepository.IsContainFactsForUser(userId);
        }
    }
}