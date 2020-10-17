using System;
using System.Collections.Generic;
using System.Linq;
using ItHappened.Domain.Statistics.Facts.ForSingleTracker;
using LanguageExt;

namespace ItHappened.Domain.Statistics.Calculators.ForMultipleTrackers
{
    public class EventsCountCalculator : IMultipleTrackersStatisticsCalculator<EventsCountFact>
    {
        public Option<EventsCountFact> Calculate(IEnumerable<EventTracker> eventTrackers)
        {
            if (!CanCalculate(eventTrackers))
                return Option<EventsCountFact>.None;

            var factName = "Зафиксировано уже N событий";
            var eventsCount = eventTrackers.SelectMany(et => et.Events).Count();
            var description = $"У вас произошло уже {eventsCount} событий!";
            var priority = Math.Log(eventsCount);

            return Option<EventsCountFact>.Some(new EventsCountFact(factName, description, priority, eventsCount));
        }
        
        private bool CanCalculate(IEnumerable<EventTracker> eventTrackers)
        {
            return eventTrackers.Any() &&
                   eventTrackers.SelectMany(et => et.Events).Count() > 5;
        }
    }
}