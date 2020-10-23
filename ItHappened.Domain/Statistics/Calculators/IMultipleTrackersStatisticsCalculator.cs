using System.Collections.Generic;
using LanguageExt;

namespace ItHappened.Domain.Statistics
{
    public interface IMultipleTrackersStatisticsCalculator
    {
        Option<IGeneralFact> Calculate(IEnumerable<EventTracker> eventTrackers);
    }
}