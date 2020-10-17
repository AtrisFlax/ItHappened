using System.Collections.Generic;
using ItHappened.Domain.Statistics.Facts.ForMultipleTrackers;
using LanguageExt;

namespace ItHappened.Domain.Statistics.Calculators.ForMultipleTrackers
{
    public interface IMultipleTrackersStatisticsCalculator<T> where T: IMultipleTrackersStatisticsFact
    {
        Option<T> Calculate(IEnumerable<EventTracker> eventTrackers);
    }
}