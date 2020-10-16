using ItHappened.Domain.Statistics.Facts.ForSingleTracker;
using LanguageExt;

namespace ItHappened.Domain.Statistics.Calculators.ForSingleTracker
{
    public interface ISingleTrackerStatisticsCalculator
    {
        Option<ISingleTrackerStatisticsFact> Calculate(EventTracker eventTracker);
    }
}