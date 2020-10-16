using ItHappend.Domain.Statistics.StatisticsFacts;
using LanguageExt;

namespace ItHappend.Domain.Statistics.SingleTrackerCalculator
{
    public interface ISingleTrackerStatisticsCalculator
    {
        Option<ISingleTrackerStatisticsFact> Calculate(EventTracker eventTracker);
    }
}