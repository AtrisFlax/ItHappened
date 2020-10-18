using System;
using System.Collections.Generic;
using ItHappened.Domain.Statistics;

namespace ItHappened.Application.Services.StatisticService
{
    public interface IStatisticsService
    {
        public IReadOnlyCollection<IMultipleTrackersStatisticsFact> GetMultipleTrackersFacts(Guid userId);
        public IReadOnlyCollection<ISingleTrackerStatisticsFact> GetSingleTrackerFacts(Guid userId,
            Guid eventTrackerId);
    }
}