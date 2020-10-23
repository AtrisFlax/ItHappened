using System;
using System.Collections.Generic;
using ItHappened.Domain;
using ItHappened.Domain.Statistics;

namespace ItHappened.Application.Services.StatisticService
{
    public class StatisticsService : IStatisticsService
    {
        private readonly IEventTrackerRepository _eventTrackerRepository;
        private readonly IMultipleTrackersFactProvider _multipleTrackersFactProvider;
        private readonly ISingleTrackerFactProvider _singleTrackerFactProvider;

        public StatisticsService(IEventTrackerRepository eventTrackerRepository,
            IMultipleTrackersFactProvider multipleTrackersFactProvider,
            ISingleTrackerFactProvider singleTrackerFactProvider)
        {
            _eventTrackerRepository = eventTrackerRepository;
            _multipleTrackersFactProvider = multipleTrackersFactProvider;
            _singleTrackerFactProvider = singleTrackerFactProvider;
        }

        public IReadOnlyCollection<IGeneralFact> GetStatisticsFactsForAllUserTrackers(Guid userId)
        {
            var eventTrackers = _eventTrackerRepository.LoadAllUserTrackers(userId);
            return _multipleTrackersFactProvider.GetFacts(eventTrackers);
        }

        public IReadOnlyCollection<ISingleTrackerFact> GetStatisticsFactsForTracker(Guid userId, Guid trackerId)
        {
            var eventTracker = _eventTrackerRepository.LoadTracker(userId);
            return _singleTrackerFactProvider.GetFacts(eventTracker);
        }
    }
}