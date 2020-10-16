using System.Collections.Generic;
using ItHappened.Domain.Statistics.Facts.ForMultipleTrackers;
using LanguageExt;

namespace ItHappened.Domain.Statistics.Calculators.ForMultipleTrackers
{
    public interface IMultipleTrackersStatisticsCalculator
    {
        Option<IMultipleTrackersStatisticsFact> Calculate(IEnumerable<EventTracker.EventTracker> eventTrackers);
    }
}