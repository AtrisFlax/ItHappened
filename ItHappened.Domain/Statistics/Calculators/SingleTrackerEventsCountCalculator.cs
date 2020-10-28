﻿using System;
using System.Collections.Generic;
using System.Linq;
using LanguageExt;

namespace ItHappened.Domain.Statistics
{
    public class SingleTrackerEventsCountCalculator : ISingleTrackerStatisticsCalculator
    {
        public Option<ISingleTrackerFact> Calculate(IReadOnlyCollection<Event> events, EventTracker tracker, DateTimeOffset now)
        {
            if (!CanCalculate(events)) 
                return Option<ISingleTrackerFact>.None;

            const string factName = "Количество событий";
            var eventsCount = events.Count;
            var description = $"Событие {tracker.Name} произошло {eventsCount} раз";
            var priority = Math.Log(eventsCount);

            return Option<ISingleTrackerFact>
                .Some(new SingleTrackerEventsCountFact(factName, description, priority, eventsCount));
        }

        private static bool CanCalculate(IEnumerable<Event> events)
        {
            return events.Any();
        }
    }
}