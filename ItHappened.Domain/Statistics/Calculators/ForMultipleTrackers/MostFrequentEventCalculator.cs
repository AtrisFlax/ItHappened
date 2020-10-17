using System;
using System.Collections.Generic;
using System.Linq;
using ItHappened.Domain.Statistics.Facts.ForMultipleTrackers;
using LanguageExt;

namespace ItHappened.Domain.Statistics.Calculators.ForMultipleTrackers
{
    public class MostFrequentEventCalculator : IMultipleTrackersStatisticsCalculator<MostFrequentEventFact>
    {
        public Option<MostFrequentEventFact> Calculate(IEnumerable<EventTracker> eventTrackers)
        {
            var enumerable = eventTrackers as EventTracker[] ?? eventTrackers.ToArray();
            if (!CanCalculate(enumerable))
                return Option<MostFrequentEventFact>.None;

            var trackingNameWithEventsPeriod = eventTrackers
                .Select(eventTracker =>
                    (trackingName : eventTracker.Name,
                    eventsPeriod : 1.0 * eventTracker
                                              .Events
                                              .GroupBy(t => t.HappensDate.Date)
                                              .Select(t => t.Key)
                                              .Count() / eventTracker.Events.Count)
                );
                 
            var trackingNameWithMinEventsPeriod =  trackingNameWithEventsPeriod
                .OrderBy(e => e.eventsPeriod)
                .FirstOrDefault();
            
            return Option<MostFrequentEventFact>
                .Some(new MostFrequentEventFact(factName,
                    description,
                    priority,
                    eventTrackersWithPeriods,
                    eventTrackerWithSmallestPeriod.eventTracker,
                    eventTrackerWithSmallestPeriod.eventsPeriod));
        }

        private bool CanCalculate(IEnumerable<EventTracker> eventTrackers) =>
            eventTrackers.Count() > 1 && eventTrackers.Count(et => et.Events.Count > 3) > 1;

        private double GetEventsPeriod(EventTracker eventTracker)
        {
            var events = eventTracker.Events;
            // ReSharper disable once PossibleLossOfFraction
            return (DateTime.Now - events
                .OrderBy(e => e.HappensDate)
                .First().HappensDate).Days / events.Count;
        }
    }
}