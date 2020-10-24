using System;
using System.Collections.Generic;
using System.Linq;

namespace ItHappened.Domain.Statistics
{
    public class StatisticGenerator : IManualStatisticGenerator, IBackgroundStatisticGenerator
    {
        private readonly IGeneralFactsRepository _generalFactsRepository;
        private readonly IGeneralFactProvider _generalFactProvider;
        private readonly ISpecificFactProvider _specificFactProvider;
        private readonly ISpecificFactsRepository _specificFactsRepository;
        private readonly IEventTrackerRepository _eventTrackerRepository;
        private readonly IUserRepository _userRepository;

        public StatisticGenerator(IGeneralFactsRepository generalFactsRepository, 
            IGeneralFactProvider generalFactProvider, 
            ISpecificFactProvider specificFactProvider, 
            ISpecificFactsRepository specificFactsRepository, IEventTrackerRepository eventTrackerRepository, IUserRepository userRepository)
        {
            _generalFactsRepository = generalFactsRepository;
            _generalFactProvider = generalFactProvider;
            _specificFactProvider = specificFactProvider;
            _specificFactsRepository = specificFactsRepository;
            _eventTrackerRepository = eventTrackerRepository;
            _userRepository = userRepository;
        }

        public void UpdateUserGeneralFacts(Guid userId)
        {
            var userTrackers = _eventTrackerRepository.LoadAllUserTrackers(userId);
            var updatedFacts = _generalFactProvider.GetFacts(userTrackers);
            _generalFactsRepository.UpdateUserGeneralFacts(updatedFacts, userId);
        }

        public void UpdateTrackerSpecificFacts(Guid trackerId)
        {
            var tracker = _eventTrackerRepository.LoadTracker(trackerId);
            if (!tracker.IsUpdated) return;
            var updatedFacts = _specificFactProvider.GetFacts(tracker);
            _specificFactsRepository.UpdateTrackerSpecificFacts(trackerId, updatedFacts);
            tracker.IsUpdated = false;
        }

        public void UpdateAllUsersGeneralFacts()
        {
            var allUsersIds = _userRepository.LoadAllUsersIds();
            foreach (var userId in allUsersIds)
            {
                UpdateUserGeneralFacts(userId);
            }
        }

        public void UpdateAllUsersSpecificFacts()
        {
            IEnumerable<EventTracker> trackers = _eventTrackerRepository.LoadAllTrackers();
            foreach (var tracker in trackers)
            {
                UpdateTrackerSpecificFacts(tracker.Id);
            }
        }
    }
    
}