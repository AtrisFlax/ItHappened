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

        public IReadOnlyCollection<IFact> GetGeneralFacts(Guid userId)
        {
            var eventTrackers = EventTrackerRepository.LoadAllUserTrackers(userId);
            return MultipleTrackersStatisticsProvider().GetFacts(eventTrackers);
        }

        public IReadOnlyCollection<IFact> GetSpecificFacts(Guid userId)
        {
            var eventTrackers = EventTrackerRepository.LoadAllUserTrackers(userId);
            var trackersFacts = new List<IFact>();
            foreach (var tracker in eventTrackers)
            {
                var factsFromTracker = SingleTrackersStatisticsProvider().GetFacts(tracker);
                var factsList = new List<IFact>(factsFromTracker);
                trackersFacts = trackersFacts.Concat(factsList).ToList();
            }

            return trackersFacts.AsReadOnly();
        }

        public IReadOnlyCollection<IFact> GetFacts(Guid userId)
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