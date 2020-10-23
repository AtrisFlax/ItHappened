﻿using System;
using System.Collections.Generic;
using System.Linq;
using LanguageExt;

namespace ItHappened.Domain.Statistics
{
    public class SingleTrackerEventsCountCalculator : ISingleTrackerStatisticsCalculator
    {
        public Option<ISingleTrackerTrackerFact> Calculate(IReadOnlyCollection<Event> events, EventTracker tracker)
        {
            if (!CanCalculate(events)) 
                return Option<ISingleTrackerTrackerFact>.None;

            const string factName = "Количество событий";
            var eventsCount = events.Count;
            var description = $"Событие {tracker.Name} произошло {eventsCount} раз";
            var priority = Math.Log(eventsCount);

            return Option<ISingleTrackerTrackerFact>
                .Some(new SingleTrackerTrackerEventsCountFact(factName, description, priority, eventsCount));
        }

        private static bool CanCalculate(IEnumerable<Event> events)
        {
            return events.Any();
        }
    }
}