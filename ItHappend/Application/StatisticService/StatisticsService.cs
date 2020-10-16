using System;
using System.Collections.Generic;
using System.Linq;
using ItHappend.Domain;
using ItHappend.Domain.Statistics.MultipleTrackersStatisticsFacts;
using ItHappend.Domain.Statistics.StatisticsFacts;
using LanguageExt;

namespace ItHappend.StatisticService
{
    public class StatisticsService : IStatisticsService
    {
        public StatisticsService(IUserRepository userRepository,
            IEventTrackerRepository eventTrackerRepository,
            IMultipleTrackersStatisticsCalculatorContainer multipleContainer,
            ISingleTrackerStatisticsCalculatorContainer singleContainer)
        {
            _userRepository = userRepository;
            _eventTrackerRepository = eventTrackerRepository;
            _multipleContainer = multipleContainer;
            _singleContainer = singleContainer;
        }
        
        public IReadOnlyCollection<Option<IMultipleTrackersStatisticsFact>> GetMultipleTrackersFacts(Guid userId)
        {
            var user = _userRepository.TryLoadUserInfo(userId);
            return _multipleContainer
                .GetFacts(user.EventTrackers)
                .Where(fact => !fact.IsNone)
                .OrderBy(fact => fact.Select(x => x.Priority))
                .ToList();
        }

        public IReadOnlyCollection<Option<ISingleTrackerStatisticsFact>> GetSingleTrackerFacts(Guid userId,
            Guid eventTrackerId)
        {
            var eventTracker = _eventTrackerRepository.LoadEventTracker(eventTrackerId);
            return _singleContainer
                .GetFacts(eventTracker)
                .Where(fact => !fact.IsNone)
                .OrderBy(fact => fact.Select(x => x.Priority))
                .ToList();
        }
        
        private readonly IUserRepository _userRepository;
        private readonly IEventTrackerRepository _eventTrackerRepository;
        private readonly IMultipleTrackersStatisticsCalculatorContainer _multipleContainer;
        private readonly ISingleTrackerStatisticsCalculatorContainer _singleContainer;
    }
}