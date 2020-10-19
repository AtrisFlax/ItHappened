using System;
using System.Collections.Generic;
using System.Linq;
using LanguageExt;

namespace ItHappened.Domain.Statistics
{
    public class EventsCountCalculator : IMultipleTrackersStatisticsCalculator
    {
        public Option<IStatisticsFact> Calculate(IEnumerable<EventTracker> eventTrackers)
        {
            if (!CanCalculate(eventTrackers))
                return Option<IStatisticsFact>.None;

            var factName = "Зафиксировано уже N событий";
            var eventsCount = eventTrackers.SelectMany(et => et.Events).Count();
            var description = $"У вас произошло уже {eventsCount} событий!";
            var priority = Math.Log(eventsCount);

            return Option<IStatisticsFact>.Some(new EventsCountFact(factName, description, priority, eventsCount));
        }

        private static bool CanCalculate(IEnumerable<EventTracker> eventTrackers)
        {
            return eventTrackers.Any() &&
                   eventTrackers.SelectMany(et => et.Events).Count() > 5;
        }
    }
}