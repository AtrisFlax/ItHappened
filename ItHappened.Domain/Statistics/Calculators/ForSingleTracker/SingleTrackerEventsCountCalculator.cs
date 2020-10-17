using ItHappened.Domain.Statistics.Facts.ForSingleTracker;
using LanguageExt;

namespace ItHappened.Domain.Statistics.Calculators.ForSingleTracker
{
    public class SingleTrackerEventsCountCalculator : ISingleTrackerStatisticsCalculator<SingleTrackerEventsCountFact>
    {
        private bool CanCalculate(EventTracker eventTracker) => 
            eventTracker.Events.Count > 0;

        public Option<SingleTrackerEventsCountFact> Calculate(EventTracker eventTracker)
        {
            if (!CanCalculate(eventTracker))
                return Option<SingleTrackerEventsCountFact>.None;

            var eventsCount = eventTracker.Events.Count;
            return Option<SingleTrackerEventsCountFact>
                .Some(new SingleTrackerEventsCountFact(eventTracker, eventsCount));
        }
    }
}