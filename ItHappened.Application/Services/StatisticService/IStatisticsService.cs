using System;
using System.Collections.Generic;
using ItHappened.Domain.Statistics;
using LanguageExt;

namespace ItHappened.Application.Services.StatisticService
{
    public interface IStatisticsService
    {
        public IReadOnlyCollection<Option<IMultipleTrackersStatisticsFact>> GetMultipleTrackersFacts(Guid userId);
        public IReadOnlyCollection<Option<ISingleTrackerStatisticsFact>> GetSingleTrackerFacts(Guid userId,
            Guid eventTrackerId);
    }
}