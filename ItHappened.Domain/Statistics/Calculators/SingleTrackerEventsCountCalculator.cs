using System;
using LanguageExt;

namespace ItHappened.Domain.Statistics
{
    public class SingleTrackerEventsCountCalculator : ISingleTrackerStatisticsCalculator
    {
        private bool CanCalculate(EventTracker eventTracker) => 
            eventTracker.Events.Count > 0;

        public Option<ISingleTrackerStatisticsFact> Calculate(EventTracker eventTracker)
        {
            if (!CanCalculate(eventTracker))
                return Option<ISingleTrackerStatisticsFact>.None;

            var factName = "Количество событий";
            var eventsCount = eventTracker.Events.Count;
            var description  = $"Событие {eventTracker.Name} произошло {eventsCount} раз";
            var priority = Math.Log(eventsCount);
            
            return Option<ISingleTrackerStatisticsFact>
                .Some(new SingleTrackerEventsCountFact(factName, description, priority, eventsCount));
        }
    }
}