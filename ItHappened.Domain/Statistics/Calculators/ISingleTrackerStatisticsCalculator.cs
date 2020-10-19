using LanguageExt;

namespace ItHappened.Domain.Statistics
{
    public interface ISingleTrackerStatisticsCalculator
    {
        Option<IStatisticsFact> Calculate(EventTracker eventTracker);
    }
}