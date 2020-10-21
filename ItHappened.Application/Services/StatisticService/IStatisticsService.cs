using System;
using System.Collections.Generic;
using ItHappened.Domain.Statistics;

namespace ItHappened.Application.Services.StatisticService
{
    public interface IStatisticsService
    {
        IReadOnlyCollection<IGeneralFact> GetGeneralTrackersFacts(Guid userId);
        IReadOnlyCollection<ISpecificFact> GetSpecificTrackerFacts(Guid userId, Guid eventTrackerId);
    }
}