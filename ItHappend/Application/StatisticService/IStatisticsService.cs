using System;
using System.Collections.Generic;
using ItHappend.Domain.Statistics.MultipleTrackersStatisticsFacts;
using ItHappend.Domain.Statistics.StatisticsFacts;
using LanguageExt;

namespace ItHappend.StatisticService
{
    public interface IStatisticsService
    {
        public IReadOnlyCollection<Option<IMultipleTrackersStatisticsFact>> GetMultipleTrackersFacts(Guid userId);
        public IReadOnlyCollection<Option<ISingleTrackerStatisticsFact>> GetSingleTrackerFacts(Guid userId,
            Guid eventTrackerId);
    }
}