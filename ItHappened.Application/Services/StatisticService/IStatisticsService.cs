using System;
using System.Collections.Generic;
using ItHappened.Domain.Statistics;

namespace ItHappened.Application.Services.StatisticService
{
    public interface IStatisticsService
    {
        IReadOnlyCollection<IGeneralFact> GetStatisticsFactsForAllUserTrackers(Guid userId);
        IReadOnlyCollection<ISingleTrackerFact> GetStatisticsFactsForTracker(Guid userId, Guid trackerId);
    }
}