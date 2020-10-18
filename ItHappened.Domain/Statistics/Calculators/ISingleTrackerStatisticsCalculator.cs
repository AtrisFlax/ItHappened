using LanguageExt;

namespace ItHappened.Domain.Statistics
{
    public interface ISingleTrackerStatisticsCalculator
    {
        Option<ISingleTrackerStatisticsFact> Calculate(EventTracker eventTracker);
    }
}