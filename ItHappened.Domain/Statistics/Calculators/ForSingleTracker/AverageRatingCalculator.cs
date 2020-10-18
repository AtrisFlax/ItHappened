using System;
using System.Linq;
using ItHappened.Domain.Statistics.Facts.ForSingleTracker;
using LanguageExt;
using LanguageExt.UnsafeValueAccess;

namespace ItHappened.Domain.Statistics.Calculators.ForSingleTracker
{
    public class AverageRatingCalculator : ISingleTrackerStatisticsCalculator<AverageRatingFact>
    {
        public Option<AverageRatingFact> Calculate(EventTracker eventTracker)
        {
            if (!CanCalculate(eventTracker)) return Option<AverageRatingFact>.None;
            var averageRating = eventTracker.Events.Average(x => x.Rating.ValueUnsafe());
            return Option<AverageRatingFact>.Some(new AverageRatingFact(
                "Среднее значение оценки",
                $"Средний рейтинг для события {eventTracker.Name} равен {averageRating}",
                Math.Sqrt(averageRating),
                averageRating
            ));
        }

        private bool CanCalculate(EventTracker eventTracker)
        {
            if (!eventTracker.HasRating)
            {
                return false;
            }

            if (eventTracker.Events.Any(@event => @event.Rating == Option<double>.None))
            {
                return false;
            }

            return eventTracker.Events.Count > 1;
        }
    }
}