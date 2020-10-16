using System.Collections.Generic;
using ItHappened.Bll.Domain.Statistics.MultipleTrackersStatisticsFacts;
using LanguageExt;

namespace ItHappened.Bll.Domain.Statistics.MultipleTrackersCalculators
{
    public interface IMultipleTrackersStatisticsCalculator
    {
        Option<IMultipleTrackersStatisticsFact> Calculate(IEnumerable<EventTracker> eventTrackers);
    }
}