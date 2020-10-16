using ItHappened.Domain.Statistics.Facts.ForSingleTracker;
using LanguageExt;

namespace ItHappened.Domain.Statistics.Calculators.ForSingleTracker
{
    public interface ISingleTrackerStatisticsCalculator<T> where T : ISingleTrackerStatisticsFact
    {
        Option<T> Calculate(EventTracker.EventTracker eventTracker);
    }
}