using System;
using System.Collections.Generic;
using System.Linq;
using LanguageExt;
using Serilog;

namespace ItHappened.Domain.Statistics
{
    public class AverageRatingCalculator : ISingleTrackerStatisticsCalculator
    {
        public Option<ISingleTrackerFact> Calculate(IReadOnlyCollection<Event> events, EventTracker tracker)
        {
            if (!CanCalculate(events))
            {
                return Option<ISingleTrackerFact>.None;
            }
            
            var averageRating = events.Select(e => e.CustomizationsParameters.Rating).Somes().Average();
            
            const string factName = "Среднее значение оценки";
            var description = $"Средний рейтинг для события {tracker.Name} равен {averageRating}";
            var priority = Math.Sqrt(averageRating);
            
            return Option<ISingleTrackerFact>.Some(new AverageRatingTrackerFact(
                factName,
                description,
                priority,
                averageRating
            ));
        }

        private static bool CanCalculate(IReadOnlyCollection<Event> events) {
            return events.Count > 1;
        }
    }
}