using ItHappened.Domain.Statistics.SingleTrackerStatisticsFacts;
using LanguageExt;

namespace ItHappened.Domain.Statistics.SingleTrackerCalculator
{
    public interface ISingleTrackerStatisticsCalculator
    {
        Option<ISingleTrackerStatisticsFact> Calculate(EventTracker eventTracker);
    }
}