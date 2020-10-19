using System;
using System.Collections.Generic;
using System.Linq;
using ItHappened.Domain;
using ItHappened.Domain.Statistics;

namespace ItHappened.Application.Services.StatisticService
{
    public class StatisticsService : IStatisticsService
    {
        private readonly IEventTrackerRepository _eventTrackerRepository;
        private readonly IMultipleTrackersStatisticsProvider _multipleTrackersStatisticsProvider;
        private readonly ISingleTrackerStatisticsProvider _singleTrackerStatisticsProvider;

        public StatisticsService(IEventTrackerRepository eventTrackerRepository)
        {
            
            var multipleTrackersStatisticsProvider = new MultipleTrackersStatisticsProvider();
            multipleTrackersStatisticsProvider.Add(new EventsCountCalculator());
            multipleTrackersStatisticsProvider.Add(new MostFrequentEventCalculator());

            var singleTrackersStatisticsProvider = new SingleTrackerStatisticsProvider();
            singleTrackersStatisticsProvider.Add(new AverageRatingCalculator());
            singleTrackersStatisticsProvider.Add(new LongestBreakCalculator());

            
            
            _eventTrackerRepository = eventTrackerRepository;
            _multipleTrackersStatisticsProvider = multipleTrackersStatisticsProvider;
            _singleTrackerStatisticsProvider = singleTrackersStatisticsProvider;
        }

        public IReadOnlyCollection<IStatisticsFact> GetMultipleTrackersFacts(Guid userId)
        {
            var eventTrackers = _eventTrackerRepository.LoadUserTrackers(userId);
            return _multipleTrackersStatisticsProvider.GetFacts(eventTrackers);
        }

        public IReadOnlyCollection<IStatisticsFact> GetSingleTrackerFacts(Guid userId)
        {
            var eventTracker = _eventTrackerRepository.LoadEventTracker(userId);
            return _singleTrackerStatisticsProvider.GetFacts(eventTracker);
        }

        public IReadOnlyCollection<IStatisticsFact> GetFactsMultipleTrackersFacts(Guid userId)
        {
            var multiFacts = GetMultipleTrackersFacts(userId);
            var singleFacts = GetSingleTrackerFacts(userId);
            return multiFacts.Concat(singleFacts).ToList().AsReadOnly();
        }
    }
}