using System.Collections.Generic;
using LanguageExt;

namespace ItHappened.Domain.Statistics
{
    public interface ISingleTrackerStatisticsCalculator
    {
        Option<ISingleTrackerFact> Calculate(EventTracker eventTracker);
    }
}