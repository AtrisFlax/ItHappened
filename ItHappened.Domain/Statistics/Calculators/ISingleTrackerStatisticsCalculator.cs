using LanguageExt;

namespace ItHappened.Domain.Statistics
{
    public interface ISingleTrackerStatisticsCalculator<T> where T : ISingleTrackerStatisticsFact
    {
        Option<T> Calculate(EventTracker eventTracker);
    }
}