using System;
using System.Collections.Generic;
using ItHappened.Domain.Statistics;

namespace ItHappened.Application.Services.StatisticService
{
    public interface IStatisticsService
    {
        IReadOnlyCollection<IMultipleTrackerTrackerFact> GetStatisticsFactsForAllTrackers(Guid userId);
        IReadOnlyCollection<ISingleTrackerTrackerFact> GetStatisticsFactsForTracker(Guid trackerId, Guid userId);
    }
}