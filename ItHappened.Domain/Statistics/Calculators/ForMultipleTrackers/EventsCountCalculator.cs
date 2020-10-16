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

            var eventsCount = eventTrackers.SelectMany(et => et.Events).Count();
            var priority = Math.Log(eventsCount);
            
            return Option<EventsCountFact>.Some(new EventsCountFact(priority, eventsCount));
        }
        
        private bool CanCalculate(IEnumerable<EventTracker> eventTrackers)
        {
            return eventTrackers.Any() &&
                   eventTrackers.SelectMany(et => et.Events).Count() > 5;
        }
    }
}