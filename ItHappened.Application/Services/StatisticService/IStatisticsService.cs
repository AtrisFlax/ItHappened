using System;
using System.Collections.Generic;
using ItHappened.Domain.Statistics;

namespace ItHappened.Application.Services.StatisticService
{
    public interface IStatisticsService
    {
        IReadOnlyCollection<IStatisticsFact> GetFacts(Guid userId);
        IReadOnlyCollection<IStatisticsFact> GetGeneralFacts(Guid userId);
        IReadOnlyCollection<IStatisticsFact> GetSpecificFacts(Guid userId);
    }
}