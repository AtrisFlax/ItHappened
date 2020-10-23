using System.Collections.Generic;
using LanguageExt;

namespace ItHappened.Domain.Statistics
{
    public interface ISingleTrackerStatisticsCalculator
    {
        public Option<ISingleTrackerTrackerFact> Calculate(IReadOnlyCollection<Event> events, EventTracker tracker);
    }
}