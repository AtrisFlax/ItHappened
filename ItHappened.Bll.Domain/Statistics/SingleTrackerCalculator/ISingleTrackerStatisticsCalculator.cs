using ItHappened.Bll.Domain.Statistics.SingleTrackerStatisticsFacts;
using LanguageExt;

namespace ItHappened.Bll.Domain.Statistics.SingleTrackerCalculator
{
    public interface ISingleTrackerStatisticsCalculator
    {
        Option<ISingleTrackerStatisticsFact> Calculate(EventTracker eventTracker);
    }
}