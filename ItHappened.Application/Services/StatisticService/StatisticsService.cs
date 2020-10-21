using System;
using System.Collections.Generic;
using ItHappened.Domain;
using ItHappened.Domain.Statistics;

namespace ItHappened.Application.Services.StatisticService
{
    public class StatisticsService : IStatisticsService
    {
        private readonly IEventTrackerRepository _eventTrackerRepository;
        private readonly IGeneralFactProvider _generalFactProvider;
        private readonly ISpecificFactProvider _specificFactProvider;

        public StatisticsService(IEventTrackerRepository eventTrackerRepository,
            IGeneralFactProvider generalFactProvider,
            ISpecificFactProvider specificFactProvider)
        {
            _eventTrackerRepository = eventTrackerRepository;
            _generalFactProvider = generalFactProvider;
            _specificFactProvider = specificFactProvider;
        }

        public IReadOnlyCollection<IGeneralFact> GetGeneralTrackersFacts(Guid userId)
        {
            var eventTrackers = _eventTrackerRepository.LoadAllUserTrackers(userId);
            return _generalFactProvider.GetFacts(eventTrackers);
        }

        public IReadOnlyCollection<ISpecificFact> GetSpecificTrackerFacts(Guid userId, Guid eventTrackerId)
        {
            var eventTracker = _eventTrackerRepository.LoadTracker(userId);
            return _specificFactProvider.GetFacts(eventTracker);
        }
    }
}