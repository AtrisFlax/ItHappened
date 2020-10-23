using System;
using System.Collections.Generic;
using ItHappened.Domain;
using ItHappened.Domain.Statistics;

namespace ItHappened.Application.Services.StatisticService
{
    public class StatisticsService : IStatisticsService
    {
        private readonly ITrackerRepository _trackerRepository;
        private readonly IMultipleTrackersFactProvider _multipleTrackersFactProvider;
        private readonly ISingleTrackerFactProvider _singleTrackerFactProvider;

        public StatisticsService(ITrackerRepository trackerRepository,
            IMultipleTrackersFactProvider multipleTrackersFactProvider,
            ISingleTrackerFactProvider singleTrackerFactProvider)
        {
            _trackerRepository = trackerRepository;
            _multipleTrackersFactProvider = multipleTrackersFactProvider;
            _singleTrackerFactProvider = singleTrackerFactProvider;
        }

        public IReadOnlyCollection<IGeneralFact> GetStatisticsFactsForAllUserTrackers(Guid userId)
        {
            var eventTrackers = _trackerRepository.LoadAllUserTrackers(userId);
            return _multipleTrackersFactProvider.GetFacts(eventTrackers);
        }

        public IReadOnlyCollection<ISingleTrackerFact> GetStatisticsFactsForTracker(Guid userId, Guid trackerId)
        {
            var eventTracker = _trackerRepository.LoadTracker(userId);
            return _singleTrackerFactProvider.GetFacts(eventTracker);

        }
    }
}