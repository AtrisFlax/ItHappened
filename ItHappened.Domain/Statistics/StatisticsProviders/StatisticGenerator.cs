using System;
using System.Collections.Generic;
using System.Linq;

namespace ItHappened.Domain.Statistics
{
    public class StatisticGenerator : IManualStatisticGenerator, IBackgroundStatisticGenerator
    {
        private readonly IMultipleFactsRepository _multipleFactsRepository;
        private readonly IMultipleTrackersFactProvider _generalFactProvider;
        private readonly ISingleTrackerFactProvider _specificFactProvider;
        private readonly ISingleFactsRepository _singleFactsRepository;
        private readonly ITrackerRepository _trackerRepository;
        private readonly IEventRepository _eventRepository;

        public StatisticGenerator(IMultipleFactsRepository multipleFactsRepository, 
            IMultipleTrackersFactProvider generalFactProvider, 
            ISingleTrackerFactProvider specificFactProvider, 
            ISingleFactsRepository singleFactsRepository, 
            ITrackerRepository trackerRepository,
            IEventRepository eventRepository)
        {
            _multipleFactsRepository = multipleFactsRepository;
            _generalFactProvider = generalFactProvider;
            _specificFactProvider = specificFactProvider;
            _singleFactsRepository = singleFactsRepository;
            _trackerRepository = trackerRepository;
            _eventRepository = eventRepository;
        }

        public void UpdateOnRequestUserGeneralFacts(Guid userId)
        {
            var userTrackers = _trackerRepository.LoadAllUserTrackers(userId);
            var trackersWithEvents = new List<TrackerWithItsEvents>();
            foreach (var tracker in userTrackers)
            {
                var events = _eventRepository.LoadAllTrackerEvents(tracker.Id);
                trackersWithEvents.Add(new TrackerWithItsEvents(tracker, events));
            }
            var updatedFacts = _generalFactProvider.GetFacts(trackersWithEvents);
            _multipleFactsRepository.UpdateUserGeneralFacts(updatedFacts, userId);
        }

        public void UpdateOnRequestTrackerSpecificFacts(Guid trackerId)
        {
            var tracker = _trackerRepository.LoadTracker(trackerId);
            var trackerEvents = _eventRepository.LoadAllTrackerEvents(trackerId);
            if (!tracker.IsUpdated) return;
            var updatedFacts = _specificFactProvider.GetFacts(trackerEvents, tracker);
            _singleFactsRepository.UpdateTrackerSpecificFacts(trackerId, updatedFacts);
            tracker.IsUpdated = false;
        }

        public void UpdateUserFacts(Guid userId)
        {
            var userTrackers = _trackerRepository.LoadAllUserTrackers(userId);
            UpdateUserGeneralFacts(userId, userTrackers);
            UpdateUserSpecificFacts(userId, userTrackers);
        }
        
        private void UpdateUserGeneralFacts(Guid userId, IEnumerable<EventTracker> userTrackers)
        {
            if (userTrackers.All(tracker => tracker.IsUpdated == false)) return;
            var trackersWithEvents = new List<TrackerWithItsEvents>();
            foreach (var tracker in userTrackers)
            {
                var events = _eventRepository.LoadAllTrackerEvents(tracker.Id);
                trackersWithEvents.Add(new TrackerWithItsEvents(tracker, events));
            }
            
            var updatedFacts = _generalFactProvider.GetFacts(trackersWithEvents);
            _multipleFactsRepository.UpdateUserGeneralFacts(updatedFacts, userId);
        }
        
        private void UpdateUserSpecificFacts(Guid userId, IEnumerable<EventTracker> userTrackers)
        {
            foreach (var tracker in userTrackers)
            {
                var trackerEvents = _eventRepository.LoadAllTrackerEvents(tracker.Id);
                if (!tracker.IsUpdated) return;
                var updatedFacts = _specificFactProvider.GetFacts(trackerEvents, tracker);
                _singleFactsRepository.UpdateTrackerSpecificFacts(tracker.Id, updatedFacts);
                tracker.IsUpdated = false;
            }
        }
    }
    
}