using System;
using System.Collections.Generic;
using ItHappened.Domain;
using ItHappened.Domain.Statistics;

namespace ItHappened.Application.Services.StatisticService
{
    public class StatisticsService : IStatisticsService
    {
        private readonly IEventTrackerRepository _eventTrackerRepository;
        private readonly IMultipleTrackersStatisticsProvider _multipleTrackersStatisticsProvider;
        private readonly ISingleTrackerStatisticsProvider _singleTrackerStatisticsProvider;

        public StatisticsService(IUserRepository userRepository,
            IEventTrackerRepository eventTrackerRepository,
            IMultipleTrackersStatisticsProvider multipleTrackersStatisticsProvider,
            ISingleTrackerStatisticsProvider singleTrackerStatisticsProvider)
        {
            _eventTrackerRepository = eventTrackerRepository;
            _multipleTrackersStatisticsProvider = multipleTrackersStatisticsProvider;
            _singleTrackerStatisticsProvider = singleTrackerStatisticsProvider;
        }

        public IReadOnlyCollection<IMultipleTrackersStatisticsFact> GetMultipleTrackersFacts(Guid userId)
        {
            var eventTrackers = _eventTrackerRepository.LoadUserTrackers(userId);
            return _multipleTrackersStatisticsProvider.GetFacts(eventTrackers);
        }

        public IReadOnlyCollection<ISingleTrackerStatisticsFact> GetSingleTrackerFacts(Guid userId,
            Guid eventTrackerId)
        {
            var eventTracker = _eventTrackerRepository.LoadEventTracker(userId);
            return _singleTrackerStatisticsProvider.GetFacts(eventTracker);
        }
    }
}