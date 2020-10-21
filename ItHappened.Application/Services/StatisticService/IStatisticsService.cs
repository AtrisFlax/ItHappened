using System;
using System.Collections.Generic;
using ItHappened.Domain.Statistics;

namespace ItHappened.Application.Services.StatisticService
{
    public interface IStatisticsService
    {
        IReadOnlyCollection<IFact> GetFacts(Guid userId);
        IReadOnlyCollection<IFact> GetGeneralFacts(Guid userId);
        IReadOnlyCollection<IFact> GetSpecificFacts(Guid userId);
    }
}