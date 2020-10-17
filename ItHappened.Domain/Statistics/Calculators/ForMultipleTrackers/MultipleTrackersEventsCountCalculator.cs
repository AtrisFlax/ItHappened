using System;
using System.Collections.Generic;
using System.Linq;
using ItHappened.Domain.Statistics.Facts.ForMultipleTrackers;
using ItHappened.Domain.Statistics.Facts.ForSingleTracker;
using LanguageExt;

namespace ItHappened.Domain.Statistics.Calculators.ForMultipleTrackers
{
    public class MultipleTrackersEventsCountCalculator : IMultipleTrackersStatisticsCalculator<MultipleTrackersEventsCountFact>
    {
        public Option<MultipleTrackersEventsCountFact> Calculate(IEnumerable<EventTracker> eventTrackers)
        {
            if (!CanCalculate(eventTrackers))
                return Option<MultipleTrackersEventsCountFact>.None;

            var eventsCount = eventTrackers.SelectMany(et => et.Events).Count();
            var priority = Math.Log(eventsCount);
            
            return Option<MultipleTrackersEventsCountFact>.Some(new MultipleTrackersEventsCountFact(eventsCount));
        }
        
        private bool CanCalculate(IEnumerable<EventTracker> eventTrackers)
        {
            return eventTrackers.Any() &&
                   eventTrackers.SelectMany(et => et.Events).Count() > 5;
        }
    }
}