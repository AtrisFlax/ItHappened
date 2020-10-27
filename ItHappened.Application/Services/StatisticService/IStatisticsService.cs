using System;
using System.Collections.Generic;
using ItHappened.Domain.Statistics;

namespace ItHappened.Application.Services.StatisticService
{
    public interface IStatisticsService
    {
        IReadOnlyCollection<IMultipleTrackersFact> GetMultipleTrackersFacts(Guid userId);
        IReadOnlyCollection<ISingleTrackerFact> GetSingleTrackerFacts(Guid eventTrackerId, Guid userId);
    }
}