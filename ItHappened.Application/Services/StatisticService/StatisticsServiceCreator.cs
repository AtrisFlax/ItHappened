using System;
using System.Collections.Generic;
using System.Linq;
using ItHappened.Domain;
using ItHappened.Domain.Statistics;

namespace ItHappened.Application.Services.StatisticService
{
    public abstract class StatisticsServiceCreator : IStatisticsService
    {
        protected readonly IEventTrackerRepository EventTrackerRepository;

        protected StatisticsServiceCreator(IEventTrackerRepository eventTrackerRepository)
        {
            EventTrackerRepository = eventTrackerRepository;
        }

        public IReadOnlyCollection<IStatisticsFact> GetMultipleTrackersFacts(Guid userId)
        {
            var eventTrackers = EventTrackerRepository.LoadUserTrackers(userId);
            return  MultipleTrackersStatisticsProvider().GetFacts(eventTrackers);
        }

        public IReadOnlyCollection<IStatisticsFact> GetSingleTrackerFacts(Guid userId)
        {
            var eventTracker = EventTrackerRepository.LoadEventFromTracker(userId);
            return  SingleTrackersStatisticsProvider().GetFacts(eventTracker);
        }

        public IReadOnlyCollection<IStatisticsFact> GetStatisticFacts(Guid userId)
        {
            var multiFacts = GetMultipleTrackersFacts(userId);
            var singleFacts = GetSingleTrackerFacts(userId);
            return multiFacts.Concat(singleFacts).ToList().AsReadOnly();
        }

        protected abstract SingleTrackerStatisticsProvider SingleTrackersStatisticsProvider();
        protected abstract MultipleTrackersStatisticsProvider MultipleTrackersStatisticsProvider();
    }
}