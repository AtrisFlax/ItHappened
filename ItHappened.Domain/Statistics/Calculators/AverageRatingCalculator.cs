using System;
using System.Collections.Generic;
using System.Linq;
using LanguageExt;
using Serilog;

namespace ItHappened.Domain.Statistics
{
    public class AverageRatingCalculator : ISingleTrackerStatisticsCalculator
    {
        public Option<ISingleTrackerTrackerFact> Calculate(IReadOnlyCollection<Event> events, EventTracker tracker)
        {
            if (!CanCalculate(events)) return Option<ISingleTrackerTrackerFact>.None;

            var averageRating = events.Average(x =>
            {
                return x.CustomizationsParameters.Rating.Match(
                    r => r,
                    () =>
                    {
                        Log.Warning("Calculate Empty Rating While Calculate AverageRatingCalculator");
                        return 0;
                    });
            });
            return Option<ISingleTrackerTrackerFact>.Some(new AverageRatingTrackerFact(
                "Среднее значение оценки",
                $"Средний рейтинг для события {tracker.Name} равен {averageRating}",
                Math.Sqrt(averageRating),
                averageRating
            ));
        }

        private static bool CanCalculate(IReadOnlyCollection<Event> events)
        {
            if (events.Any(@event => @event.CustomizationsParameters.Rating == Option<double>.None)) return false;
            return events.Count > 1;
        }
    }
}