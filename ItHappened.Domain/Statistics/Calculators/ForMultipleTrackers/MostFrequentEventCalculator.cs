using System;
using System.Collections.Generic;
using System.Linq;
using ItHappened.Domain.Statistics.Facts.ForMultipleTrackers;
using LanguageExt;

namespace ItHappened.Domain.Statistics.Calculators.ForMultipleTrackers
{
    public class MostFrequentEventCalculator : IMultipleTrackersStatisticsCalculator<MostFrequentEvent>
    {
        public Option<MostFrequentEvent> Calculate(IEnumerable<EventTracker> eventTrackers)
        {
            var enumerable = eventTrackers as EventTracker[] ?? eventTrackers.ToArray();
            if (!CanCalculate(enumerable))
                return Option<MostFrequentEvent>.None;

            var eventTrackersWithPeriods = enumerable
                .Select(eventTracker => (eventTracker, eventsPeriod: GetEventsPeriod(eventTracker)))
                .ToList();

            var eventTrackerWithSmallestPeriod = eventTrackersWithPeriods
                .OrderBy(x => x.eventsPeriod)
                .First();

            const string factName = "Самое частое событие";
            
            var description =
                $"Чаще всего у вас происходит событие {eventTrackerWithSmallestPeriod.eventTracker.Name}" +
                $" - раз в {eventTrackerWithSmallestPeriod.eventsPeriod} дней";
            
            var priority = 10 / eventTrackerWithSmallestPeriod.eventsPeriod;
            return Option<MostFrequentEvent>
                .Some(new MostFrequentEvent(factName,
                    description,
                    priority,
                    eventTrackersWithPeriods,
                    eventTrackerWithSmallestPeriod.eventTracker,
                    eventTrackerWithSmallestPeriod.eventsPeriod));
        }

        private bool CanCalculate(IEnumerable<EventTracker> eventTrackers)
        {
            return eventTrackers.Count() > 1 &&
                   eventTrackers.Count(et => et.Events.Count > 3) > 1;
        }

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