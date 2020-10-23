using System.Collections.Generic;
using LanguageExt;

namespace ItHappened.Domain.Statistics
{
    public interface ISingleTrackerStatisticsCalculator
    {
        public Option<ISingleTrackerFact> Calculate(IReadOnlyCollection<Event> events, EventTracker tracker);
    }
}