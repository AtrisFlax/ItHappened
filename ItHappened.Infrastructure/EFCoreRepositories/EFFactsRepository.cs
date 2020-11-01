using System;
using System.Collections.Generic;
using AutoMapper;
using ItHappened.Domain.Statistics;
using ItHappened.Infrastructure.Mappers;

namespace ItHappened.Infrastructure.EFCoreRepositories
{
    public class EFFactsRepository
    {
        private readonly ItHappenedDbContext _context;
        private readonly IMapper _mapper;

        public EFFactsRepository(ItHappenedDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public IReadOnlyCollection<ISingleTrackerFact> LoadTrackerSpecificFacts(Guid trackerId)
        {
            throw new NotImplementedException();
        }

        public void LoadTrackerSpecificFacts(Guid trackerId, IReadOnlyCollection<ISingleTrackerFact> facts)
        {
            // foreach (var fact in facts)
            // {
            //     switch (fact)
            //     {
            //         
            //         case AverageRatingTrackerFact concreteFact:
            //         {
            //             _context..Add(eventDto);
            //             _mapper.Map<AverageRatingTrackerFactDto>(concreteFact);
            //             break;
            //         }
            //     }
            // }

            throw new NotImplementedException();
        }

        public bool IsContainFactForTracker(Guid trackerId)
        {
            throw new NotImplementedException();
        }

        public void LoadTrackerSpecificFacts(Guid trackerId, IReadOnlyCollection<IMultipleTrackersFact> facts)
        {
            throw new NotImplementedException();
        }

        public IReadOnlyCollection<IMultipleTrackersFact> LoadUserGeneralFacts(Guid userId)
        {
            throw new NotImplementedException();
        }
    }
}