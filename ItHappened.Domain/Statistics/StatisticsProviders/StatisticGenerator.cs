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
        
        public void UpdateOnRequestTrackerSpecificFacts(Guid trackerId)
        {
            if (!_trackerRepository.IsContainTracker(trackerId)) return;
            var tracker = _trackerRepository.LoadTracker(trackerId);
            var trackerEvents = _eventRepository.LoadAllTrackerEvents(trackerId);
            if (!tracker.IsUpdated || !trackerEvents.Any()) return;
            var updatedFacts = _specificFactProvider.GetFacts(trackerEvents, tracker);
            if (!updatedFacts.Any()) return;
            _singleFactsRepository.UpdateTrackerSpecificFacts(trackerId, updatedFacts);
            tracker.IsUpdated = false;
        }
        
        public void UpdateUserFacts(Guid userId)
        {
            var userTrackers = _trackerRepository.LoadAllUserTrackers(userId);
            if (userTrackers.All(tracker => tracker.IsUpdated == false)) return;
            UpdateUserGeneralFacts(userId, userTrackers);
            UpdateUserSpecificFacts(userTrackers);
        }
        
        private void UpdateUserGeneralFacts(Guid userId, IEnumerable<EventTracker> userTrackers)
        {
            var trackersWithEvents = new List<TrackerWithItsEvents>();
            foreach (var tracker in userTrackers)
            {
                var events = _eventRepository.LoadAllTrackerEvents(tracker.Id);
                trackersWithEvents.Add(new TrackerWithItsEvents(tracker, events));
            }
            
            var updatedFacts = _generalFactProvider.GetFacts(trackersWithEvents);
            _multipleFactsRepository.UpdateUserGeneralFacts(userId, updatedFacts);
        }
        
        private void UpdateUserSpecificFacts(IEnumerable<EventTracker> userTrackers)
        {
            var updatedTrackers = userTrackers.Where(tracker => tracker.IsUpdated);
            foreach (var tracker in updatedTrackers)
            {
                var trackerEvents = _eventRepository.LoadAllTrackerEvents(tracker.Id);
                var updatedFacts = _specificFactProvider.GetFacts(trackerEvents, tracker);
                _singleFactsRepository.UpdateTrackerSpecificFacts(tracker.Id, updatedFacts);
                tracker.IsUpdated = false;
            }
        }
        
    }
    
}