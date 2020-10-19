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

        public IReadOnlyCollection<IStatisticsFact> GetGeneralFacts(Guid userId)
        {
            var eventTrackers = EventTrackerRepository.LoadUserTrackers(userId);
            return MultipleTrackersStatisticsProvider().GetFacts(eventTrackers);
        }

        public IReadOnlyCollection<IStatisticsFact> GetSpecificFacts(Guid userId)
        {
            var eventTrackers = EventTrackerRepository.LoadUserTrackers(userId);
            var trackersFacts = new List<IStatisticsFact>();
            foreach (var tracker in eventTrackers)
            {
                var factsFromTracker = SingleTrackersStatisticsProvider().GetFacts(tracker);
                var factsList = new List<IStatisticsFact>(factsFromTracker);
                trackersFacts = trackersFacts.Concat(factsList).ToList();
            }

            return trackersFacts.AsReadOnly();
        }

        public IReadOnlyCollection<IStatisticsFact> GetFacts(Guid userId)
        {
            var multiFacts = GetGeneralFacts(userId);
            var singleFacts = GetSpecificFacts(userId);
            var allFacts = multiFacts.Concat(singleFacts).ToList();
            return  allFacts.OrderByDescending(fact => fact.Priority).ToList().AsReadOnly();
        }

        protected abstract SingleTrackerStatisticsProvider SingleTrackersStatisticsProvider();
        protected abstract MultipleTrackersStatisticsProvider MultipleTrackersStatisticsProvider();
    }
}