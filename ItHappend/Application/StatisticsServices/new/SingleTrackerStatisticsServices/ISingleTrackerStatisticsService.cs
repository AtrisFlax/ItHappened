using System;

namespace ItHappend.StatisticsServices
{
    public interface ISingleTrackerStatisticsService<TFact>
    {
        TFact GetStatistics(Guid userId, Guid trackerId);
    }
}