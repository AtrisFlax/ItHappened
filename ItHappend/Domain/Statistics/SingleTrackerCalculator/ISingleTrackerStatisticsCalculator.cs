using ItHappend.Domain.Statistics.StatisticsFacts;
using LanguageExt;

namespace ItHappend.Domain.Statistics.SingleTrackerCalculator
{
    public interface ISingleTrackerStatisticsCalculator<TFact>
    {
        Option<TFact> Calculate(EventTracker eventTracker);
    }
}