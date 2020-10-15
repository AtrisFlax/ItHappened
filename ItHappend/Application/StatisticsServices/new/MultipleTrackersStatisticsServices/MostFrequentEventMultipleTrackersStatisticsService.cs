using System;
using ItHappend.Domain;
using ItHappend.Domain.Calculators;

namespace ItHappend.StatisticsServices
{
    public class MostFrequentEventMultipleTrackersStatisticsService : IMultipleTrackersStatisticsService<MostFrequentEvent>
    {
        private readonly IUserRepository _userRepository;
        private readonly IMultipleTrackersStatisticsCalculator<MostFrequentEvent> _multipleTrackersStatisticsCalculator;

        public MostFrequentEventMultipleTrackersStatisticsService(IUserRepository userRepository,
            IMultipleTrackersStatisticsCalculator<MostFrequentEvent> multipleTrackersStatisticsCalculator)
        {
            _userRepository = userRepository;
            _multipleTrackersStatisticsCalculator = multipleTrackersStatisticsCalculator;
        }

        public MostFrequentEvent GetStatistics(Guid userId)
        {
            throw new NotImplementedException();
        }
    }
}