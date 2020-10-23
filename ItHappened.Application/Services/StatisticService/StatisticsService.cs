using System;
using System.Collections.Generic;
using System.Linq;
using ItHappened.Domain;
using ItHappened.Domain.Statistics;

namespace ItHappened.Application.Services.StatisticService
{
    public class StatisticsService : IStatisticsService
    {
        private readonly ITrackerRepository _trackerRepository;
        private readonly IEventRepository _eventRepository;
        private readonly IMultipleTrackersFactProvider _multipleTrackersFactProvider;
        private readonly ISingleTrackerFactProvider _singleTrackerFactProvider;

        public StatisticsService(IEventRepository eventRepository, ITrackerRepository trackerRepository,
            ISingleTrackerFactProvider singleTrackerFactProvider,
            IMultipleTrackersFactProvider multipleTrackersFactProvider)
        {
            _trackerRepository = trackerRepository;
            _eventRepository = eventRepository;
            _multipleTrackersFactProvider = multipleTrackersFactProvider;
            _singleTrackerFactProvider = singleTrackerFactProvider;
        }

        public IReadOnlyCollection<IMultipleTrackerTrackerFact> GetStatisticsFactsForAllTrackers(Guid userId)
        {
            var trackers = _trackerRepository.LoadAllUserTrackers(userId);
            var a = trackers.Select(x =>
            {
                var tracker = _trackerRepository.LoadTracker(x.Id);
                var events = _eventRepository.LoadAllTrackerEvents(x.Id);
                return new TrackerWithItsEvents(tracker, events);
            }).ToList();
            return _multipleTrackersFactProvider.GetFacts(a.AsReadOnly());
        }

        public IReadOnlyCollection<ISingleTrackerTrackerFact> GetStatisticsFactsForTracker(Guid trackerId, Guid userId)
        {
            var tracker = _trackerRepository.LoadTracker(trackerId);
            var events = _eventRepository.LoadAllTrackerEvents(trackerId);
            return _singleTrackerFactProvider.GetFacts(events, tracker);
        }
    }
}