using System;

namespace ItHappend.StatisticsServices
{
    public interface IMultipleTrackersStatisticsService<TFact>
    {
        TFact GetStatistics(Guid userId);
    }
}