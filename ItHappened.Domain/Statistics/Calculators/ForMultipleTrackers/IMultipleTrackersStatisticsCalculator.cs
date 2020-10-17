using System.Collections.Generic;
using LanguageExt;

namespace ItHappened.Domain.Statistics
{
    public interface IMultipleTrackersStatisticsCalculator<T> where T: IMultipleTrackersStatisticsFact
    {
        Option<T> Calculate(IEnumerable<EventTracker> eventTrackers);
    }
}